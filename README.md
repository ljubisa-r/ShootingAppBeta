
Here are instruction to setup localy solution:

******Aplication GlossaryApp has three part:
  1. GlossaryAPI (.net 9.0.305)
  2. GlossaryWeb (uses Vue.js 3)
  3. SQL DB Glossary

Extract zip.file in folder build solution in VS GlossaryAPI

1. GlossaryAPI 
in file "..\GlossaryAPI\appsettings.json"
You sholud update connection string to your local server with DBuser 
"User Id = sa; Password = xxx"

2. GlossaryWeb (uses Vue.js 3)
Install node.js  
in folder ..\glossaryWeb open terminal and exec command npm run dev


3. SQL DB Glossary
Execute script on local SQL server "ScriptCreteDBandTablesGlossary.sql"
Script will create DB Glossar and tebles with pre filled rows in table.
