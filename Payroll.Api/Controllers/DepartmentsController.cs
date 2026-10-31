using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Api.Data;
using Payroll.Api.Models;

namespace Payroll.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public DepartmentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Department>>> GetAll()
    {
        return await _db.Departments.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Department>> Create(Department department)
    {
        _db.Departments.Add(department);
        await _db.SaveChangesAsync();
        return Ok(department);
    }
}           
