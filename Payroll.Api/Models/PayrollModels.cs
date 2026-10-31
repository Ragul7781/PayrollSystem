using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Payroll.Api.Models;

[Index(nameof(Year), nameof(Month), IsUnique = true)]
public class PayrollRun
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public DateTime GeneratedAt { get; set; }
    public decimal TotalGross { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal TotalNet { get; set; }
    public List<Payslip> Payslips { get; set; } = new();
}

public class Payslip
{
    public int Id { get; set; }
    public int PayrollRunId { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal LossOfPayDays { get; set; }
    public decimal GrossEarnings { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetPay { get; set; }
    public List<PayslipLine> Lines { get; set; } = new();
}

public class PayslipLine
{
    public int Id { get; set; }
    public int PayslipId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public ComponentType Type { get; set; }
    public decimal Amount { get; set; }
}