using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Api.Data;
using Payroll.Api.Models;

namespace Payroll.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalaryComponentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public SalaryComponentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<SalaryComponent>>> GetAll()
    {
        return await _db.SalaryComponents.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<SalaryComponent>> Create(SalaryComponent component)
    {
        _db.SalaryComponents.Add(component);
        await _db.SaveChangesAsync();
        return Ok(component);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SalaryComponent updated)
    {
        var component = await _db.SalaryComponents.FindAsync(id);
        if (component == null)
            return NotFound();

        component.Name = updated.Name;
        component.Type = updated.Type;
        component.Calculation = updated.Calculation;
        component.Value = updated.Value;
        component.IsActive = updated.IsActive;

        await _db.SaveChangesAsync();
        return Ok(component);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var component = await _db.SalaryComponents.FindAsync(id);
        if (component == null)
            return NotFound();

        _db.SalaryComponents.Remove(component);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}