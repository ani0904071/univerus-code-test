# 👤 Person API with Integration Tests, React, and MS SQL

A sample ASP.NET Core Web API project for managing Persons and their associated PersonTypes.  
Includes full integration tests using xUnit and runs in Docker via Docker Compose.

---

1. 📦 Clone the repository

Download or clone the repo:

    git clone https://github.com/ani0904071/universu-code-test.git

2. 🛠 Build and Start the Services  (Docker must be installed on the Windows/WSL/Linux System, ex:  Docker version 28.1.1, build 4eba377) 

This will Build and run the MSSQL, PersonApi Web API service, Run integration tests from TestPerson, Run React respectively ( wait for a while to finish )

    > cd univerus-code-test 
    (Make sure you're on the main branch, type ls/dir to see the file: docker-compose.yml)
    > docker compose up --build -d

3. 📋 View Test Results (Optional)

You can check the logs from the test container:
  
    > docker logs sqlserver
    > docker logs personapi
    > docker logs testperson
    > docker logs front-end

4. 🚀 Interaction with the Project

The services will run on following url:port, paste front-end address to start using the application
  
    front-end -> http://localhost:3000
    api server -> http://localhost:5045
    mssql -> localhost:1433 (username: sa  password: Passw0rd123! )

5. 📖 Swagger

On this url, you can use the Rest end points
  
    http://localhost:5045/swagger/index.html


6. 💻 Standalone Development(Optional)
   
[ Node.js(node-v24.2.0-x64), ASP.Net(dotnet-sdk-8.0.411-win-x64) must be installed in the system ]

🌐 Frontend (frontendPerson)

    > cd frontendPerson
    > npm install
    > npm run dev

    Then open:
    ➡️ http://localhost:8080

🧱 Backend API (PersonApi)

(SQL Server must be running via SSMS or Dokcer)

    > cd PersonApi
    > dotnet clean
    > dotnet run --launch-profile "http"

🛠 Database Setup (SQL Server must be running via SSMS or Dokcer)

    Run these commands after SQL Server is up:
    
    
    > cd PersonApi
    > dotnet tool install --global dotnet-ef
    > dotnet ef migrations add YourChoiceOfMigrationName(ex: Initial)
    > dotnet ef database update

🧪 Test Project (TestPerson)

You can open the PersonApi.sln project in Visual Studio from PersonApi folder

To run or debug tests:

    Open PersonsControllerTest.cs or PersonTypesControllerTest.cs

    Right-click a test method and select Run Test or Debug Test

    ✅ Visual Studio will detect and run tests via xUnit + FluentAssertions.
