using Visitor.AdministrativeDivisions;

namespace Visitor.Visitors
{
    public interface IPrintVisitor
    {
	    void Visit(Country country);
	    void Visit(State state);
	    void Visit(City city);
    }
}
