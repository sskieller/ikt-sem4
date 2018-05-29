﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FWPS.Models
{
    /////////////////////////////////////////////////
    /// Poomba item model
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
