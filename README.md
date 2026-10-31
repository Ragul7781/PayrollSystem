# Payroll Management System

A basic full-stack payroll application: employees, departments, salary components, loss-of-pay entry, and monthly payroll runs with payslips.



## Tech stack

- **Backend:** ASP.NET Core Web API (C#, .NET 10)
- **Database:** MySQL 8 with Entity Framework Core (code-first migrations)
- **Frontend:** single HTML page with plain JavaScript (served by the API)

## Features

- Employee and department management with validation
- Configurable salary components (fixed amount or percentage of basic)
- Monthly loss-of-pay (LOP) days per employee
- Monthly payroll run that generates a payslip for every active employee
- Saved payslips with a line-by-line breakdown of earnings and deductions
- Duplicate protection: payroll can only run once per month

## How payroll is calculated

```
Gross       = basic salary + active earning components
LOP amount  = gross / days in month x LOP days
Deductions  = active deduction components + LOP amount
Net pay     = gross - deductions
```

## API endpoints

| Resource | Endpoints |
|---|---|
| Employees | `GET/POST /api/employees`, `GET/PUT/DELETE /api/employees/{id}` |
| Departments | `GET/POST /api/departments` |
| Salary components | `GET/POST /api/salarycomponents`, `PUT/DELETE /api/salarycomponents/{id}` |
| Attendance | `GET /api/attendance?year=&month=`, `POST /api/attendance` |
| Payroll | `POST /api/payroll/run?year=&month=`, `GET /api/payroll`, `GET/DELETE /api/payroll/{id}` |

Sample requests are in `Payroll.Api/Payroll.Api.http` (VS Code REST Client extension).

## Project structure

```
Payroll.Api/
  Controllers/   API endpoints
  Models/        Entities (Employee, Department, SalaryComponent, ...)
  Data/          AppDbContext (EF Core)
  Migrations/    Database migrations
  wwwroot/       Frontend page
```

## Run locally

1. Install the .NET SDK and MySQL, then create the database: `CREATE DATABASE payroll_db;`
2. In `Payroll.Api`, copy `appsettings.Development.example.json` to `appsettings.Development.json` and enter your MySQL password.
3. From `Payroll.Api`, run:
   ```
   dotnet tool install --global dotnet-ef
   dotnet ef database update
   dotnet run
   ```
4. Open http://localhost:5283

## Possible improvements

- Login with roles (Admin, HR, Employee)
- Payslip PDF download
- Deactivate employees from the UI
- Department-wise payroll reports