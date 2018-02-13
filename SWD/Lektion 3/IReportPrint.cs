using System;

namespace ReportGenerator
{
	interface IReportPrint
	{
		print(List<Employee> employees)
	}

	public class ReportNameFirst : IReportPrint
	{
		public print(List<Employee> employees)
		{
			foreach(var employee in employees)
			{
				{
					Console.WriteLine("------------------");
					Console.WriteLine("Name: {0}", e.Name);
					Console.WriteLine("Salary: {0}", e.Salary);
					Console.WriteLine("------------------");
				}
			}
		}
	}

	public class ReportSalaryFirst : IReportPrint
	{
		public print(List<Employee> employees)
		{
			foreach(var employee in employees)
			{
				{
							Console.WriteLine("------------------");
							Console.WriteLine("Salary: {0}", e.Salary);
							Console.WriteLine("Name: {0}", e.Name);
							Console.WriteLine("------------------");
				}
			}
		}
	}
}
