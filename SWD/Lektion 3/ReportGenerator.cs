using System;
using System.Collections.Generic;

namespace ReportGenerator
{
    internal class ReportGenerator
    {
        private readonly EmployeeDB _employeeDb;
        private IReportPrint _currentOutputFormat;


        public ReportGenerator(EmployeeDB employeeDb)
        {
            if (employeeDb == null) throw new ArgumentNullException("employeeDb");
            _currentOutputFormat = new ReportNameFirst;
            _employeeDb = employeeDb;
        }


        public void CompileReport()
        {
            var employees = new List<Employee>();
            Employee employee;

            _employeeDb.Reset();

            // Get all employees
            while ((employee = _employeeDb.GetNextEmployee()) != null)
            {
                employees.Add(employee);
            }

            // All employees collected - let's output them
            _currentOutputFormat.print(employees);
        }


        public void SetOutputFormat(IReportPrint format)
        {
            _currentOutputFormat = format;
        }
    }
}