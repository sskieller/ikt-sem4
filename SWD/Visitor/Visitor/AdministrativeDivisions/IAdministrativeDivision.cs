using System;
using System.Collections.Generic;
using System.Text;
using Visitor.Visitors;

namespace Visitor.AdministrativeDivisions
{
    public interface IAdministrativeDivision
    {
	    void Accept(IPrintVisitor visitor);
		string Name { get; set; }
    }
}
