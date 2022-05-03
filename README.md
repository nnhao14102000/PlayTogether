[![HaoNN - Continuous Integration](https://github.com/nnhao14102000/PlayTogether/actions/workflows/haonn.yaml/badge.svg)](https://github.com/nnhao14102000/PlayTogether/actions/workflows/haonn.yaml)
# Play Together API

## Technical vs Tools

This project is API build base on these technical:

- .NET 5 Web API
- C#
- Clean Architecture
- SQL

Tools use to develop project:

- Visual Studio 2022 - Community
- Visual Studio Code
- GitHub and GitHub Actions for CI/CD
- SQL server, Microsoft SQL Server Management Studio 18 - for working with Database

## Clone and Implement in Local

- Clone newest code (from branch test):  
`git clone -b test https://github.com/nnhao14102000/PlayTogether.git`
- Clone stable code (from branch master):  
`git clone https://github.com/nnhao14102000/PlayTogether.git`

- Modify the connection strings in **appsettings.Development.json**    
Default is: <br> `Server=(local);Database=PlayTogetherDb;Trusted_Connection=True;`  
Example: <br> `Server=YOUR_SQL_SERVER_NAME;Database=NAME_OF_DB;User=sa;Password=YOUR_PASSWORD;Trusted_Connection=False;Trusted_Connection=False;MultipleActiveResultSets=true;`

- Open project with VSCode (*Recommend*) or Visual Studio  
    - Open Terminal: `` Ctrl + ` `` for (VSCode) or  ` View -> Terminal ` (Visual Studio)
    - Type: `cd src` (go to src directory)
    - Type: `cd PlayTogether.Api` (go to Api project)
    - Type: `dotnet run` (run project)  

## Reference
- Architecture
    - [.NET Core REST API Design | Youtube](https://www.youtube.com/watch?v=TEeb0Hba8jI&list=PLKwiLOxvy13_IEpQ7iZPFiC3ejzfvD28f)  
    - [Clean Architecture Solution Template for .NET 6 | GitHub](https://github.com/jasontaylordev/CleanArchitecture)  
    - [Common web application architectures | Microsoft Document](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)  

- Code
    - [CI/CD with GitHub Actions and .NET 5 | Youtube](https://youtu.be/R5ppadIsGbA)
    - [.NET Core 3.1 MVC REST API | Youtube](https://youtu.be/fmvcAzHpsk8)

- Machine Learning with ML.NET
    - [Tutorial: Build a movie recommender using matrix factorization with ML.NET | Microsoft Document](https://docs.microsoft.com/vi-vn/dotnet/machine-learning/tutorials/movie-recommendation)
    - [Matrix Factorization Collaborative Filtering | Machine Learning cơ bản | VietNamese Language](https://machinelearningcoban.com/2017/05/31/matrixfactorization/)

- Momo Wallet Payment
    - [Instant Payment Notification](https://developers.momo.vn/v2/#/docs/aiov2/?id=instant-payment-notification)
    - [Sample Code Momo](https://github.com/momo-wallet/payment)