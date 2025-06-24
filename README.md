# 👤 Person API with Integration Tests, React, and MS SQL

A sample ASP.NET Core Web API project for managing Persons and their associated PersonTypes.  
Includes full integration tests using xUnit and runs in Docker via Docker Compose.

---

1. 📦 Clone the repository

Download or clone the repo:

    git clone https://github.com/ani0904071/universu-code-test.git
    cd universu-code-test 
    (Make sure you're on the main branch.)

2. 🛠 Build and Start the Services 

This will Build and run the MSSQL, PersonApi Web API service, Run integration tests from TestPerson, Run React respectively ( wait for a while to finish)

    docker compose up --build -d

3. 📋 View Test Results (Optional)

You can check the logs from the test container:
  
    docker logs sqlserver
    docker logs personapi
    docker logs testperson
    docker logs front-end

4.  Interaction with the project

The services will run on following url:port, paste front-end address to start using the application
  
    front-end-> http://localhost:3000
    api server-> http://localhost:5045
    mssql-> 1433, (username: sa  password: Passw0rd123! )
