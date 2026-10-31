using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Api.Data;
using Payroll.Api.Models;

namespace Payroll.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly AppDbContext _db;

    public AttendanceController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<MonthlyAttendance>>> Get(int? year, int? month)
    {
        var query = _db.MonthlyAttendances.Include(a => a.Employee).AsQueryable();

        if (year != null)
            query = query.Where(a => a.Year == year);
        if (month != null)
            query = query.Where(a => a.Month == month);

        return await query.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<MonthlyAttendance>> Save(MonthlyAttendance input)
    {
        if (!await _db.Employees.AnyAsync(e => e.Id == input.EmployeeId))
            return BadRequest("The selected employee does not exist.");

        var existing = await _db.MonthlyAttendances.FirstOrDefaultAsync(a =>
            a.EmployeeId == input.EmployeeId && a.Year == input.Year && a.Month == input.Month);

        if (existing == null)
        {
            input.Employee = null;
            _db.MonthlyAttendances.Add(input);
            existing = input;
        }
        else
        {
            existing.LossOfPayDays = input.LossOfPayDays;
        }

        await _db.SaveChangesAsync();
        return Ok(existing);
    }
}