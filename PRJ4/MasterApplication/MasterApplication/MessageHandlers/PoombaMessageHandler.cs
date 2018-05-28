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
    public class PoombaMessageHandler : IMessageHandler
    {
	    private static System.Timers.Timer _wakeUpTimer;
	    private static System.Timers.Timer _sleepTimer;
	    private readonly WebApiConnector _connector = new WebApiConnector();

	    static PoombaMessageHandler()
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
                //Sent from Morning Sun
				case "ModuleOn": //Poomba has been turned on
					LightIsOn(message);
					break;

				case "ModuleOff": //Poomba has been turned off
					LightIsOff(message);
					break;

                //Sent from Web
				case "On":
                case "on":
                    TurnOn(message);
					break;
	            case "Off":
                case "off":
	                TurnOff(message);
	                break;
                case "UpdateTime":
	                UpdateTime(message);
	                break;

				default:
					//Do not handle
					break;

	        }
        }

        #region Poomba-related handlers

        private void LightIsOn(string message)
        {
            Console.WriteLine("Morning Sun: Received \"on\" from module");
            Console.WriteLine(message);
            //Send message to DB that light is now on
            PoombaItem lightItem = new PoombaItem() { Command = "TurnedOn", IsOn = true };
            _connector.PostItem("Light/", JsonConvert.SerializeObject(lightItem));
        }

        private void LightIsOff(string message)
        {
            Console.WriteLine("Morning Sun: Received \"off\" from module");
            Console.WriteLine(message);
            //Send message to DB that light is now on
            PoombaItem lightItem = new PoombaItem() { Command = "TurnedOff", IsOn = false };
            _connector.PostItem("Light/", JsonConvert.SerializeObject(lightItem));
        }

        #endregion


        #region WebAPI-related handlers

        private void TurnOn(string message)
        {
            //Ask Poomba to turn on
            FwpsPublisher.PublishMessage("Poomba.CmdOn", "");
        }

        private void TurnOff(string message)
        {
            //Ask Poomba to turn off
            FwpsPublisher.PublishMessage("Poomba.CmdOff","");
        }

        #endregion


        #region Timer-related methods

        private static void UpdateTime(string message)
        {
            Console.WriteLine("Morning Sun: Asked to update timers");
            WebApiConnector connector = new WebApiConnector();
            //Get latest lightObject from database
            string json = connector.GetItem("Light/Newest");
            PoombaItem light = JsonConvert.DeserializeObject<PoombaItem>(json);

            //Find time for next sleep time
            TimeSpan timeToTurnOff = light.SleepTime - DateTime.Now;
            TimeSpan timeToTurnOn = light.WakeUpTime - DateTime.Now;

            // Increment TimeSpan by one day until the day matches with current day
            while (timeToTurnOff < TimeSpan.Zero)
                timeToTurnOff = timeToTurnOff + TimeSpan.FromDays(1);

            while (timeToTurnOn < TimeSpan.Zero)
                timeToTurnOn = timeToTurnOn + TimeSpan.FromDays(1);

            Console.WriteLine("Morning Sun: Timer for wakeup is now in: {0} hours", timeToTurnOn.TotalHours);
            Console.WriteLine("Morning Sun: Timer for sleep is now in: {0} hours", timeToTurnOff.TotalHours);

            //Set up timer for automatic sleep
            _sleepTimer?.Dispose();
            _wakeUpTimer?.Dispose();

            _sleepTimer = new System.Timers.Timer(timeToTurnOff.TotalMilliseconds);
            _sleepTimer.Elapsed += Sleep;
            _sleepTimer.AutoReset = false;
            _sleepTimer.Start();


            _wakeUpTimer = new System.Timers.Timer(timeToTurnOn.TotalMilliseconds);
            _wakeUpTimer.Elapsed += WakeUp;
            _wakeUpTimer.AutoReset = false;
            _wakeUpTimer.Start();
        }

        private static void WakeUp(object sender, ElapsedEventArgs e)
        {
			FwpsPublisher.PublishMessage("Poomba.CmdOn", "");

            Console.WriteLine("Light waking up");

            //Rearm timer to start in one day
            _wakeUpTimer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
            _wakeUpTimer.Start();
        }

        private static void Sleep(object sender, ElapsedEventArgs e)
        {
	        FwpsPublisher.PublishMessage("Poomba.CmdOff", "");
			Console.WriteLine("Light sleeping");

            //Rearm timer to start in one day
            _sleepTimer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
            _sleepTimer.Start();
        }

        #endregion

    }
}
