using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.Services;
using System.Data.Services.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using StackExchange.DataExplorer.Models.StackEntities;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using StackExchange.DataExplorer.Helpers;
using StackExchange.DataExplorer.Models;
using System.Data.Common;

namespace StackExchange.DataExplorer
{
    public class OData : DataService<Entities>
    {
        const int ConnectionPoolSize = 10;

        private static Regex _ipAddress = new Regex(@"\b([0-9]{1,3}\.){3}[0-9]{1,3}$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        private static bool IsPrivateIP(string s) {
            return (s.StartsWith("192.168.") || s.StartsWith("10.") || s.StartsWith("127.0.0."));
        }

        /// <summary>
        /// retrieves the IP address of the current request -- handles proxies and private networks
        /// </summary>
        public static string GetRemoteIP(NameValueCollection ServerVariables) {
            var ip = ServerVariables["REMOTE_ADDR"]; // could be a proxy -- beware
            var ipForwarded = ServerVariables["HTTP_X_FORWARDED_FOR"];

            // check if we were forwarded from a proxy
            if (ipForwarded.HasValue()) {
                ipForwarded = _ipAddress.Match(ipForwarded).Value;
                if (ipForwarded.HasValue() && !IsPrivateIP(ipForwarded))
                    ip = ipForwarded;
            }

            return ip.HasValue() ? ip : "X.X.X.X";
        }


        public static string GetRemoteIP() {
            NameValueCollection ServerVaraibles;

            // This is a nasty hack so we don't crash the non-request test cases
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                ServerVaraibles = HttpContext.Current.Request.ServerVariables;
            }
            else
            {
                ServerVaraibles = new NameValueCollection();
            }

            return GetRemoteIP(ServerVaraibles);
        }

        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.SetEntitySetPageSize("*", 50);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
            config.UseVerboseErrors = true;
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", 
            Justification = "If we dispose our connection our data source will be hosed")]
        protected override Entities CreateDataSource()
        {
            return null;
        }

        protected override void OnStartProcessingRequest(ProcessRequestArgs args)
        {
            base.OnStartProcessingRequest(args);

        }
    }
}