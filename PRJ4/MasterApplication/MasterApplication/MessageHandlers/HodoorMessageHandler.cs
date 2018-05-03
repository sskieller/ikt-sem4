using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MasterApplication.Models;
using MasterApplication.Threads;
using Newtonsoft.Json;

namespace MasterApplication.MessageHandlers
{
    public class HodoorMessageHandler : IMessageHandler
    {
        private readonly WebApiConnector _connector = new WebApiConnector();
        public void HandleMessage(string message, string topic)
        {
            switch (topic)
            {
                //Module specific handlers
                case "ModuleLocked":
                    HodoorWasLocked(message);
                    break;
                case "ModuleUnlocked":
                    HodoorWasUnlocked(message);
                    break;

                case "CmdUnlock":
                    UnlockHodoor(message);
                    break;
                case "CmdLock":
                    LockHodoor(message);
                    break;
                
            }
        }


        #region Hodoor module related handlers

        private void HodoorWasUnlocked(string message)
        {
            Console.WriteLine("Hodoor: Received \"Unlocked\" from module");
            Console.WriteLine();

            //Send message to DB that Hodoor was unlocked
            HodoorItem item = new HodoorItem(){Command = "ModuleUnlocked", OpenStatus = true};
            _connector.PostItem("Hodoor/", JsonConvert.SerializeObject(item));
            SignalRClient.Instance.UpdateEntityCondition("Hodoor", "Unlocked");
        }

        private void HodoorWasLocked(string message)
        {
            Console.WriteLine("Hodoor: Received \"Locked\" from module");
            Console.WriteLine();

            //Send message to DB that Hodoor was unlocked
            HodoorItem item = new HodoorItem() { Command = "ModuleLocked", OpenStatus = false };
            _connector.PostItem("Hodoor/", JsonConvert.SerializeObject(item));
            SignalRClient.Instance.UpdateEntityCondition("Hodoor", "Locked");
        }

        #endregion

        #region WebApi related handlers

        private void UnlockHodoor(string message)
        {
            //Unlock Hodoor
           FwpsPublisher.PublishMessage("Hodoor.CmdUnlock", "");
        }

        private void LockHodoor(string message)
        {
            //Lock Hodoor
            FwpsPublisher.PublishMessage("Hodoor.CmdLock", "");
        }

        #endregion

 



    }
}
