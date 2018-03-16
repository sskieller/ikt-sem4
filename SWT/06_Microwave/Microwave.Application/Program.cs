using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Application
{
	class Program
	{
		static void Main(string[] args)
		{
            // Output
            Output output = new Output();
            // Display
		    IDisplay display = new Display(output);
            // CookController
            IPowerTube powerTube = new PowerTube(output);
		    ITimer timer = new Timer();
            ICookController cookController =
                new CookController(timer, display, powerTube);
            // UserInterface
		    IButton powerButton = new Button();
            IButton timerButton = new Button();
            IButton startCancelButton = new Button();
		    IDoor door = new Door();
		    ILight light = new Light(output);
		    IUserInterface ui = new UserInterface(powerButton, timerButton, startCancelButton,
		        door, display, light, cookController);

            // UC 1
            door.Open();
            // UC 4
		    door.Close();

		    RunUC6ToEnd(powerButton, timerButton, startCancelButton, timer, door);


            // Extension 1 - Start from UC6
            door.Open();
            door.Close();

            powerButton.Press();
            startCancelButton.Press();

		    RunUC6ToEnd(powerButton, timerButton, startCancelButton, timer, door);

            // Extension 1 - Start from UC15
            door.Open();
            door.Close();

            powerButton.Press();
            startCancelButton.Press();

            RunUC15ToEnd(door);
		}

	    public static void RunUC6ToEnd(IButton powerButton, 
	        IButton timerButton, 
	        IButton startCancelButton, 
	        ITimer timer,
	        IDoor door)
	    {
	        // UC 6, Powerlevel 200
	        for (int i = 0; i < 3; i++)
	        {
	            powerButton.Press();
	        }
	        // UC 7, Timer set to 2 minutes
	        for (int i = 0; i < 2; i++)
	        {
	            timerButton.Press();
	        }

	        // UC 8
	        startCancelButton.Press();

	        // UC 9-14
	        while (timer.TimeRemaining > 0)
	        {
	            // Do nothing
	        }

	        // UC 15
	        door.Open();
	        // UC 18
	        door.Close();
        }

	    private static void RunUC15ToEnd(IDoor door)
	    {
            // UC 15
            door.Open();
            // UC 18
            door.Close();
	    }
	}
}
