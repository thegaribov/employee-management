# Purpose
Demonstration of dynamic filtering on entities

# About system
Basic employee management system. Features :
1. Create new department
2. Edit department details
3. View details of department
4. Delete department from system
5. Get all departments and apply filters
6. Create new employee
7. Edit employee details
8. View details of employee
9. Delete employee from system
10. Get all employee and apply filters

# Technical Stack
- ASP.NET Core 5.0 (with .NET Core 5.0)
- Entity Framework Core
- .NET Core Native DI
- FluentValidator
- AutoMapper
- SQL Server
- Newtonsoft.Json 
- Dynamic LINQ
- Swagger UI

# Design Patterns
- Unit Of Work
- Repository & Generic Repository
- ORM
- Inversion of Control / Dependency injection

# Sotware architecture
- N-tier architecture

# Prerequirements
- Visual Studio 2019+
- .NET Core 5
- EF Core
- Docker
- Docker-compose

# How To Run
- Clone repository and open `employee-management` folder
- Docker desktop should be active in Windows OS
- Run docker : `docker-compose -f _development/docker-compose.yml up --build -d`
- Application will be started on : `http://localhost:5000/`
- I added additional container (`portrainer`) to manage other containers, so you can check it :  `http://localhost:9000/`  
