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
    /////////////////////////////////////////////////
    /// Message handler for Poomba. 
    /////////////////////////////////////////////////
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
        /////////////////////////////////////////////////
        /// Message handler for Poomba. 
        /////////////////////////////////////////////////
        public void HandleMessage(string message, string topic)
        {
	        if (topic == null)
		        return;

	        switch (topic)
	        {
                //Sent from Poomba
				case "ModuleOn": //Poomba has been turned on
					PoombaIsOn(message);
					break;

				case "ModuleOff": //Poomba has been turned off
					PoombaIsOff(message);
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

        /////////////////////////////////////////////////
        /// Handler function for when Poomba turns on
        /////////////////////////////////////////////////
        private void PoombaIsOn(string message)
        {
            Console.WriteLine("Poomba: Received \"on\" from module");
            Console.WriteLine(message);
            //Send message to DB that Poomba is now on
        }

        /////////////////////////////////////////////////
        /// Handler function for when Poomba turns off
        /////////////////////////////////////////////////
        private void PoombaIsOff(string message)
        {
            Console.WriteLine("Poomba: Received \"off\" from module");
            Console.WriteLine(message);
            //Send message to DB that Poomba is now on
        }

        #endregion


        #region WebAPI-related handlers

        /////////////////////////////////////////////////
        /// Handler function for when the Web Api requests
        /// to turn on Poomba
        /////////////////////////////////////////////////
        private void TurnOn(string message)
        {
            //Ask Poomba to turn on
            Console.WriteLine("Received \"on\" Poomba");
            FwpsPublisher.PublishMessage("Poomba.CmdOn", "");
        }

        /////////////////////////////////////////////////
        /// Handler function for when the Web Api requests
        /// to turn off Poomba
        /////////////////////////////////////////////////
        private void TurnOff(string message)
        {
            Console.WriteLine("Received \"off\" Poomba");
            //Ask Poomba to turn off
            FwpsPublisher.PublishMessage("Poomba.CmdOff","");
        }

        #endregion


        #region Timer-related methods
        /////////////////////////////////////////////////
        /// Handler function for when the Web Api requests
        /// to set the timers on Poomba
        /////////////////////////////////////////////////
        private static void UpdateTime(string message)
        {
            Console.WriteLine("Poomba: Asked to update timers");
            WebApiConnector connector = new WebApiConnector();
            //Get latest PoombaObject from database
            string json = connector.GetItem("Poomba/Newest");
            PoombaItem poomba = JsonConvert.DeserializeObject<PoombaItem>(json);

            //Find time for next sleep time
            TimeSpan timeToTurnOff = poomba.SleepTime - DateTime.Now;
            TimeSpan timeToTurnOn = poomba.WakeUpTime - DateTime.Now;

            // Increment TimeSpan by one day until the day matches with current day
            while (timeToTurnOff < TimeSpan.Zero)
                timeToTurnOff = timeToTurnOff + TimeSpan.FromDays(1);

            while (timeToTurnOn < TimeSpan.Zero)
                timeToTurnOn = timeToTurnOn + TimeSpan.FromDays(1);

            Console.WriteLine("Poomba: Timer for wakeup is now in: {0} hours", timeToTurnOn.TotalHours);
            Console.WriteLine("Poomba: Timer for sleep is now in: {0} hours", timeToTurnOff.TotalHours);

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

        /////////////////////////////////////////////////
        /// Handler function for when the Timer is triggerd,
        /// and turns on Poomba
        /////////////////////////////////////////////////
        private static void WakeUp(object sender, ElapsedEventArgs e)
        {
			FwpsPublisher.PublishMessage("Poomba.CmdOn", "");

            Console.WriteLine("Poomba waking up");

            //Rearm timer to start in one day
            _wakeUpTimer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
            _wakeUpTimer.Start();
        }

        /////////////////////////////////////////////////
        /// Handler function for when the Timer is triggerd,
        /// and turns off Poomba
        /////////////////////////////////////////////////
        private static void Sleep(object sender, ElapsedEventArgs e)
        {
	        FwpsPublisher.PublishMessage("Poomba.CmdOff", "");
			Console.WriteLine("Poomba sleeping");

            //Rearm timer to start in one day
            _sleepTimer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
            _sleepTimer.Start();
        }

        #endregion

    }
}
