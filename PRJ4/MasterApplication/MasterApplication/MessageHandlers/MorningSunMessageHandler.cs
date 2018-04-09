using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using MasterApplication.Models;
using MasterApplication.Threads;
using RabbitMQ.Client.Impl;
using Newtonsoft.Json;

namespace MasterApplication.MessageHandlers
{
    public class MorningSunMessageHandler : IMessageHandler
    {
	    private static System.Timers.Timer _wakeUpTimer;
	    private static System.Timers.Timer _sleepTimer;
	    private readonly WebApiConnector _connector = new WebApiConnector();

	    static MorningSunMessageHandler()
	    {
			UpdateTime(null);
		}
		
		//Create map of topics here
        public void HandleMessage(string message, string topic)
        {
	        if (topic == null)
		        return;

	        switch (topic)
	        {
				case "ModuleOn":
					LightIsOn(message);
					break;

				case "ModuleOff":
					LightIsOff(message);
					break;

				case "UpdateTime":
					UpdateTime(message);
					break;

				case "WebOff":
					break;
				case "WebOn":
					break;

                case "Module": //Nothing important here
                    break;

                case "Hello":
                    Console.WriteLine("Received Hello from MorningSun");
                    break;

				default:
					UnknownMessage();
					break;

	        }
        }

	    private void LightIsOn(string message)
	    {
		    Console.WriteLine("Morning Sun: Received \"on\" from module");

			//Send message to DB that light is now on
			LightItem lightItem = new LightItem(){Command = "TurnedOn", IsOn = true};
		    _connector.PostItem("Light/",JsonConvert.SerializeObject(lightItem));

	    }

	    private void LightIsOff(string message)
	    {
		    Console.WriteLine("Morning Sun: Received \"off\" from module");
			//Send message to DB that light is now on
			LightItem lightItem = new LightItem() { Command = "TurnedOff", IsOn = false };
		    _connector.PostItem("Light/", JsonConvert.SerializeObject(lightItem));
		}

	    private static void UpdateTime(string message)
	    {
		    WebApiConnector connector = new WebApiConnector();
			//Get latest lightObject from database
			string json = connector.GetItem("Light/Newest");
		    LightItem light = JsonConvert.DeserializeObject<LightItem>(json);

		    //Find time for next sleep time
		    TimeSpan timeToTurnOff = light.SleepTime - DateTime.Now;
		    TimeSpan timeToTurnOn = light.WakeUpTime - DateTime.Now;

		    // Increment TimeSpan by one day until the day matches with current day
		    while (timeToTurnOff < TimeSpan.Zero)
			    timeToTurnOff = timeToTurnOff + TimeSpan.FromDays(1);

		    while (timeToTurnOn < TimeSpan.Zero)
			    timeToTurnOn = timeToTurnOn + TimeSpan.FromDays(1);

		    //Set up timer for automatic sleep
		    _sleepTimer?.Dispose();
		    _wakeUpTimer?.Dispose();

		    _sleepTimer = new System.Timers.Timer(timeToTurnOff.TotalMilliseconds);
		    _sleepTimer.Elapsed += Sleep;
		    _sleepTimer.AutoReset = false;
		    _sleepTimer.Start();


		    _wakeUpTimer = new System.Timers.Timer(timeToTurnOff.TotalMilliseconds);
		    _wakeUpTimer.Elapsed += WakeUp;
		    _wakeUpTimer.AutoReset = false;
		    _wakeUpTimer.Start();
		}

	    private void UnknownMessage()
	    {
			Console.WriteLine("Morning Sun: Unknown Message");
	    }

	    private static void WakeUp(object sender, ElapsedEventArgs e)
	    {
			Console.WriteLine("Light waking up");

			//Rearm timer to start in one day
		    _wakeUpTimer.Interval = TimeSpan.FromSeconds(2).TotalMilliseconds;
			_wakeUpTimer.Start();
	    }

	    private static void Sleep(object sender, ElapsedEventArgs e)
	    {
			Console.WriteLine("Light sleeping");

		    //Rearm timer to start in one day
		    _sleepTimer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
		    _sleepTimer.Start();
		}
    }
}
