using System;
using System.Collections.Generic;
using System.Text;

namespace MasterApplication.Controllers
{
    public static class ControllerFactory
    {
	    public static IController GetController(string controllerName)
	    {
		    switch (controllerName)
		    {
				case "Morning Sun":
					return new MorningSunController();

				default:
					throw new ControllerCreationException("Did not recognize Controller name");
		    }
	    }


    }

	public class ControllerCreationException : Exception
	{
		public ControllerCreationException(string message)
			: base(message)
		{

		}
	}
}
