using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Api.Data;
using Payroll.Api.Models;

namespace Payroll.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayrollController : ControllerBase
{
    private readonly AppDbContext _db;

    public PayrollController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("run")]
    public async Task<ActionResult<PayrollRun>> Run(int year, int month)
    {
        if (month < 1 || month > 12 || year < 2000 || year > 2100)
            return BadRequest("Enter a valid year and month.");

        if (await _db.PayrollRuns.AnyAsync(r => r.Year == year && r.Month == month))
            return Conflict("Payroll for this month has already been run.");

        var components = await _db.SalaryComponents.Where(c => c.IsActive).ToListAsync();
        var employees = await _db.Employees.Where(e => e.IsActive).ToListAsync();
        var attendance = await _db.MonthlyAttendances
            .Where(a => a.Year == year && a.Month == month)
            .ToListAsync();

        int daysInMonth = DateTime.DaysInMonth(year, month);
        var run = new PayrollRun { Year = year, Month = month, GeneratedAt = DateTime.UtcNow };

        foreach (var emp in employees)
        {
            var slip = new Payslip { EmployeeId = emp.Id, BasicSalary = emp.BasicSalary };

            slip.Lines.Add(new PayslipLine
            {
                Name = "Basic Salary",
                Type = ComponentType.Earning,
                Amount = emp.BasicSalary
            });

            foreach (var c in components)
            {
                var amount = c.Calculation == CalculationType.PercentOfBasic
                    ? Math.Round(emp.BasicSalary * c.Value / 100, 2)
                    : c.Value;

                slip.Lines.Add(new PayslipLine { Name = c.Name, Type = c.Type, Amount = amount });
            }

            slip.GrossEarnings = slip.Lines
                .Where(l => l.Type == ComponentType.Earning)
                .Sum(l => l.Amount);

            var lopDays = attendance.FirstOrDefault(a => a.EmployeeId == emp.Id)?.LossOfPayDays ?? 0;
            slip.LossOfPayDays = lopDays;

            if (lopDays > 0)
            {
                var lopAmount = Math.Round(slip.GrossEarnings / daysInMonth * lopDays, 2);
                slip.Lines.Add(new PayslipLine
                {
                    Name = "Loss of Pay",
                    Type = ComponentType.Deduction,
                    Amount = lopAmount
                });
            }

            slip.TotalDeductions = slip.Lines
                .Where(l => l.Type == ComponentType.Deduction)
                .Sum(l => l.Amount);

            slip.NetPay = slip.GrossEarnings - slip.TotalDeductions;
            run.Payslips.Add(slip);
        }

        run.TotalGross = run.Payslips.Sum(p => p.GrossEarnings);
        run.TotalDeductions = run.Payslips.Sum(p => p.TotalDeductions);
        run.TotalNet = run.Payslips.Sum(p => p.NetPay);

        _db.PayrollRuns.Add(run);
        await _db.SaveChangesAsync();

        return await GetRun(run.Id);
    }

    [HttpGet]
    public async Task<ActionResult<List<PayrollRun>>> GetRuns()
    {
        return await _db.PayrollRuns
            .OrderByDescending(r => r.Year)
            .ThenByDescending(r => r.Month)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PayrollRun>> GetRun(int id)
    {
        var run = await _db.PayrollRuns
            .Include(r => r.Payslips).ThenInclude(p => p.Employee)
            .Include(r => r.Payslips).ThenInclude(p => p.Lines)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (run == null)
            return NotFound();

        return run;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRun(int id)
    {
        var run = await _db.PayrollRuns.FindAsync(id);
        if (run == null)
            return NotFound();

        _db.PayrollRuns.Remove(run);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}