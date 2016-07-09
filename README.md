#SQLite Kei
##Description
SQLite Kei is a database management tool for SQLite databases for .NET 

##Existing Features
- Create, open, close, refresh and remove databases
- Manage multiple databases simultaneously
- Manage tables, including the option to use useful editors/wizards that assist you in the creation of tables and select queries
- Option to search your queried select results
- Simple query editor to write plain SQL to your databases

##Planned Features
- Rename tables
- Extended query editor
- Improve and extend existing editors/wizards
- Manage Views
- Manage Indexes
- Manage Triggers
- In-Line editing of query results
- Copy and move tables between databases
- Print and csv export
- And more

##Technical infos & Technologies used
- C# on .NET Framework 4.5.2
- WPF
- SQLite
- log4net
- NUnit

##Contribution and participation
Note: As this is a project for studying purposes, I currently won't accept any pull requests until the tool is further down the release-path. Sorry and thanks for understanding!

If you still want to help out, feel free to test the newest versions and report any issues, may it be bugs or (feature) suggestions.
You can report any issue on my Taiga.io project [here](https://tree.taiga.io/project/shaezonai-sqlite-kei/issues "here") or check out the [project backlog](https://tree.taiga.io/project/shaezonai-sqlite-kei/backlog "backlog") to see what's currently in progress and planned.

##Project Structure
- **SQLiteKei** - The main project which represents the UI and business layer.
- **SQLiteKei.DataAccess** - Provides functionality to access an sqlite database and to create SQL queries using custom QueryBuilders

###Test Projects
- **SQLiteKei.UnitTests** - The unit tests for the main project.
- **SQLiteKei.DataAccess.UnitTests** - The unit tests for the DataAccess project.
- **SQLiteKei.IntegrationTests** - The main project for integration tests of all projects. Note: tests need the NUnit 3 Runner to be started from VS locally. R# does not support the [OneTimeSetup] and [OneTimeTearDown] features of NUnit which replaced [TestFixtureSetup] and [TestFixtureTearDown] respectively.

##Trivia
- About the project name: I was looking for a working title and noticed that all the cool stuff like Manager, Management, Studio, Administrator, etcetera all were in use already. Since I like to use random Japanese words as working titles for my projects I picked the Japanese Kanji 系 (written as 'kei') which means "system".
- It started out as a side project for me to learn different technologies. Hooray for messy beginner code! This tool is **not** written in MVVM style, though I consider refactoring it into that direction in the future. 
- I organize myself using [Taiga.io](https://taiga.io "Taiga.io"), an open source project management tool that supports Scrum and Kanban styled procedures. It's a really neat and powerful tool, you should check them out.
