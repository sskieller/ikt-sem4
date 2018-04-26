using Visitor.Visitors;

namespace Visitor.AdministrativeDivisions
{
	public class State : IAdministrativeDivision
	{
		public string Name { get; set; }
		public string StateCapital { get; set; }
		public void Accept(IPrintVisitor visitor)
		{
			visitor.Visit(this);
		}

		public State(string name, string stateCapital)
		{
			Name = name;
			StateCapital = stateCapital;
		}
	}
}