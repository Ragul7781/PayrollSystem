using System.ComponentModel.DataAnnotations;

namespace Payroll.Api.Models;

public class Employee
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    public DateTime DateOfJoining { get; set; }

    [Range(1, 10000000)]
    public decimal BasicSalary { get; set; }

    public bool IsActive { get; set; } = true;

    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
}