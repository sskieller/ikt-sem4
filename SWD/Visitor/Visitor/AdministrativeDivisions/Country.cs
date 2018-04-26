using Visitor.Visitors;

namespace Visitor.AdministrativeDivisions
{
	public class Country : IAdministrativeDivision
	{
		public string Name { get; set; }
		public string FlagColor { get; set; }
		public void Accept(IPrintVisitor visitor)
		{
			visitor.Visit(this);
		}

		public Country(string name, string flagColor)
		{
			Name = name;
			FlagColor = flagColor;
		}
	}
}