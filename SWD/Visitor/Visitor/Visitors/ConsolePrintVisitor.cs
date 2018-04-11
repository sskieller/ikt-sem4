using System;
using Visitor.AdministrativeDivisions;

namespace Visitor.Visitors
{
	public class ConsolePrintVisitor : IPrintVisitor
	{
		public void Visit(Country country)
		{
			Console.WriteLine("Country of: {0}", country.Name);
			Console.WriteLine("Flag color: {0}", country.FlagColor);
			Console.WriteLine();
		}

		public void Visit(State state)
		{
			Console.WriteLine("State of: {0}", state.Name);
			Console.WriteLine("State capital: {0}", state.StateCapital);
			Console.WriteLine();
		}

		public void Visit(City city)
		{
			Console.WriteLine("City of: {0}", city.Name);
			Console.WriteLine("City Mayor: {0}", city.MayorName);
			Console.WriteLine();
		}
	}
}