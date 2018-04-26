using System;
using Visitor.AdministrativeDivisions;

namespace Visitor.Visitors
{
	public class FilePrintVisitor : IPrintVisitor
	{
		public void Visit(Country country)
		{
			Console.WriteLine("Country of \"{0}\" written to file", country.Name);
			Console.WriteLine();
		}

		public void Visit(State state)
		{
			Console.WriteLine("State of \"{0}\" written to file", state.Name);
			Console.WriteLine();
		}

		public void Visit(City city)
		{
			Console.WriteLine("City of \"{0}\" written to file", city.Name);
			Console.WriteLine();
		}
	}
}