﻿namespace MasterApplication.Models
{
    /////////////////////////////////////////////////
    /// Item representing the SnapBoxItem on the WebApi
    /////////////////////////////////////////////////
    public class SnapBoxItem : ItemBase
    {
        public string SnapBoxId { get; set; }
        public string PowerLevel { get; set; }
        public bool MailReceived { get; set; }
        public string ReceiverEmail { get; set; }
        public string Checksum { get; set; }
    }
}