# Play Together API

## Technical vs Tools

This project is API build base on these technical:

- .NET 6 Web API
- C#
- Clean Architecture
- SQL

Tools use to develop project:

- Visual Studio 2022 - Community
- Visual Studio Code
- Github and Github action for CI/CD
- SQL server, Microsoft SQL Server Management Studio 18 - for working with Database

## Clone and Implement in Local

- Clone newest code (from branch test):  
`git clone -b test https://github.com/nnhao14102000/PlayTogether.git`

- Modify the connection strings in **appsettings.Development.json**    
Default is: <br> `Server=(local);Database=PlayTogetherDb;Trusted_Connection=True;`  
Example: <br> `Server=YOUR_SQL_SERVER_NAME;Database=NAME_OF_DB;User=sa;Password=YOUR_PASSWORD;Trusted_Connection=False;Trusted_Connection=False;MultipleActiveResultSets=true;`

- Open project with VSCode (*Recommend*) or Visual Studio  
    - Open Terminal: ` Ctrl+ ` \` for (VSCode) or  ` View -> Terminal ` (Visual Studio)
    - Type: `cd src` (go to src directory)
    - Type: `cd PlayTogether.Api` (go to Api project)
    - Type: `dotnet run` (run project)