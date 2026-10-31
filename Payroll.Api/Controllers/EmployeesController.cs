using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Api.Data;
using Payroll.Api.Models;

namespace Payroll.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _db;

    public EmployeesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Employee>>> GetAll()
    {
        return await _db.Employees.Include(e => e.Department).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        var employee = await _db.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
            return NotFound();

        return employee;
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> Create(Employee employee)
    {
        if (!await DepartmentExists(employee.DepartmentId))
            return BadRequest("The selected department does not exist.");

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
        return Ok(employee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Employee updated)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null)
            return NotFound();

        if (!await DepartmentExists(updated.DepartmentId))
            return BadRequest("The selected department does not exist.");

        employee.EmployeeCode = updated.EmployeeCode;
        employee.FirstName = updated.FirstName;
        employee.LastName = updated.LastName;
        employee.Email = updated.Email;
        employee.DateOfJoining = updated.DateOfJoining;
        employee.BasicSalary = updated.BasicSalary;
        employee.IsActive = updated.IsActive;
        employee.DepartmentId = updated.DepartmentId;

        await _db.SaveChangesAsync();
        return Ok(employee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null)
            return NotFound();

        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private async Task<bool> DepartmentExists(int? departmentId)
    {
        if (departmentId == null)
            return true;

        return await _db.Departments.AnyAsync(d => d.Id == departmentId);
    }
}