using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Payroll.Api.Models;

[Index(nameof(EmployeeId), nameof(Year), nameof(Month), IsUnique = true)]
public class MonthlyAttendance
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    [Range(2000, 2100)]
    public int Year { get; set; }

    [Range(1, 12)]
    public int Month { get; set; }

    [Range(0, 31)]
    public decimal LossOfPayDays { get; set; }
}