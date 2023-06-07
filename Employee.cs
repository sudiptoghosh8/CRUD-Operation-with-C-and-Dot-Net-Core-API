// Model for tblEmployee
public class Employee
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public string EmployeeCode { get; set; }
    public decimal EmployeeSalary { get; set; }
}

// Model for tblEmployeeAttendance
public class EmployeeAttendance
{
    public int EmployeeId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public bool IsPresent { get; set; }
    public bool IsAbsent { get; set; }
    public bool IsOffday { get; set; }
}





// PUT: api/employees/{employeeId}/employeecode
[HttpPut("api/employees/{employeeId}/employeecode")]
public IActionResult UpdateEmployeeCode(int employeeId, [FromBody] string employeeCode)
{
    // Check if the employee code already exists
    if (tblemplyee.Employees.Any(e => e.EmployeeCode == employeeCode))
    {
        return BadRequest("Employee code already exists.");
    }

    var employee = tblemplyee.Employees.Find(employeeId);
    if (employee == null)
    {
        return NotFound();
    }

    employee.EmployeeCode = employeeCode;
    tblemplyee.SaveChanges();

    return Ok("Employee code updated successfully.");
}





// GET: api/employees/sort/salary
[HttpGet("api/employees/sort/salary")]
public IActionResult GetAllEmployeesBySalary()
{
    var employees = tblemplyee.Employees.OrderByDescending(e => e.EmployeeSalary).ToList();
    return Ok(employees);
}




// GET: api/employees/absent
[HttpGet("api/employees/absent")]
public IActionResult GetEmployeesWithAbsentDays()
{
    var employees = tblemplyee.Employees
        .Where(e => tblemplyee.EmployeeAttendances.Any(a => a.EmployeeId == e.EmployeeId && a.IsAbsent))
        .ToList();

    return Ok(employees);
}




// GET: api/employees/attendance
[HttpGet("api/employees/attendance")]
public IActionResult GetMonthlyAttendanceReport()
{
    var report = tblemplyee.Employees
        .Select(e => new
        {
            e.EmployeeName,
            MonthName = "",
            TotalPresent = tblemplyee.EmployeeAttendances.Count(a => a.EmployeeId == e.EmployeeId && a.IsPresent),
            TotalAbsent = tblemplyee.EmployeeAttendances.Count(a => a.EmployeeId == e.EmployeeId && a.IsAbsent),
            TotalOffday = tblemplyee.EmployeeAttendances.Count(a => a.EmployeeId == e.EmployeeId && a.IsOffday)
        })
        .ToList();

    return Ok(report);
}





