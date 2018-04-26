using Visitor.Visitors;

namespace Visitor.AdministrativeDivisions
{
	public class City : IAdministrativeDivision
	{
		public string Name { get; set; }
		public string MayorName { get; set; }
		public void Accept(IPrintVisitor visitor)
		{
			visitor.Visit(this);
		}

		public City(string name, string mayorName)
		{
			Name = name;
			MayorName = mayorName;
		}
	}
}