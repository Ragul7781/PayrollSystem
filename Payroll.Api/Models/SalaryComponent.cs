using System.ComponentModel.DataAnnotations;

namespace Payroll.Api.Models;

public enum ComponentType { Earning = 1, Deduction = 2 }
public enum CalculationType { Fixed = 1, PercentOfBasic = 2 }

public class SalaryComponent
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public ComponentType Type { get; set; }
    public CalculationType Calculation { get; set; }

    [Range(0, 10000000)]
    public decimal Value { get; set; }

    public bool IsActive { get; set; } = true;
}