using System;
using System.Text.RegularExpressions;
using MasterApplication.Models;
using MasterApplication.Threads;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MasterApplication.MessageHandlers
{
    /////////////////////////////////////////////////
    /// Message handler for Poomba. 
    /////////////////////////////////////////////////
    public class SnapBoxMessageHandler : IMessageHandler
    {
        /////////////////////////////////////////////////
        /// Message handler for Poomba. SnapBox only receives
        /// one kind of message. This message is then split
        /// using a RegEx, such that 2 values are extracted.
        /// These values will then decide if mail is received,
        /// and what the powerlevel of the battery is.
        /// These values are then posted to the WebApi
        /////////////////////////////////////////////////
        public void HandleMessage(string message, string topic = "")
        {
            Console.WriteLine("Snap Box message: {0}, Topic: {1}", message, topic);

            var data = Regex.Split(message, @"\D+");

            // Length might be 3 or 2, depending on the Regex implementation...

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = data[i].Trim();
            }

            if (data.Length == 3 && string.IsNullOrEmpty(data[0]))
            {
                if (data[1] == "0" && data[2] == "0") return; // False message from I2C stutter

                var snapboxitem = new SnapBoxItem() {MailReceived = data[1] == "1", PowerLevel = data[2], ReceiverEmail = "simonvu@post.au.dk" };

                var webapi = new WebApiConnector();

                webapi.PostItem("snapbox/", JsonConvert.SerializeObject(snapboxitem));
            }
            else if (data.Length == 2)
            {
                if (data[0] == "0" && data[1] == "0") return; // False message from I2C stutter

                var snapboxitem = new SnapBoxItem() { MailReceived = data[0] == "1", PowerLevel = data[1], ReceiverEmail = "simonvu@post.au.dk" };

                var webapi = new WebApiConnector();

                webapi.PostItem("snapbox/", JsonConvert.SerializeObject(snapboxitem));
            }
            else 
                throw new ArgumentException(string.Format("SnapBox Received Wrong Input -- Data length: {0} -- Data: {1}", data.Length, data.ToString()));
        }

        
    }
}