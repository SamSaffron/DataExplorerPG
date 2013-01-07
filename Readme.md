This is a fork of http://code.google.com/p/stack-exchange-data-explorer/

### Why it exists? 

1. Strips out some Stack Exchange specific stuff out of DataExplorer (footer and legal)
2. Moves the repo to Git
3. Adds PostgreSQL support to Data Explorer (only querying for now)


### Why not simply integrate this into core? 

Main reason is time, it would be rather risky to add my changes as they are into the main branch, additionally allowing for custom footers and legal sections would be rather complicated. 

### What it does? 

Data Explorer allows you to run arbitrary queries against any SQL Server or Postgres database in a modern Web frontend. 

Some cool features:

1. AJAX ui meaning queries run in the background and update the UI when done
2. Full history for all queries you ever ran AND full revision list per query
3. CSV download
4. Text or Grid based results
5. Schema explorer 
6. Graphing support


### Demo

A demo/working site with the official release is at: http://data.stackexchange.com

<img src="http://cdn.community-tracker.com/uploads/site_20/2/se_shot.PNG">
