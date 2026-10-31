# Payroll Management System

A basic full-stack practice project.

**Stack:** ASP.NET Core Web API (C#, .NET 10), EF Core with MySQL, plain HTML/JavaScript frontend.

**Features:** employees, departments, salary components, monthly loss-of-pay entry, monthly payroll runs with payslips.

## Run it

1. Install the .NET SDK and MySQL, then create a database: `CREATE DATABASE payroll_db;`
2. In `Payroll.Api`, create `appsettings.Development.json` with your own connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Port=3306;Database=payroll_db;User=root;Password=YOUR_PASSWORD;"
     }
   }
   ```
3. From `Payroll.Api`, run: