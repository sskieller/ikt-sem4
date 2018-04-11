using System;
using Visitor.AdministrativeDivisions;
using Visitor.Visitors;

namespace Visitor
{
    class Program
    {
        static void Main(string[] args)
        {
            IPrintVisitor consoleVisitor = new ConsolePrintVisitor();
	        Country usa = new Country("USA", "RedWhiteBlue");
			State california = new State("California", "Sacramento");
			City newYork = new City("New York", "Bill de Blasio");

			usa.Accept(consoleVisitor);
			california.Accept(consoleVisitor);
			newYork.Accept(consoleVisitor);

			usa.Accept(new FilePrintVisitor());
			california.Accept(new FilePrintVisitor());
			newYork.Accept(new FilePrintVisitor());
        }
    }
}
