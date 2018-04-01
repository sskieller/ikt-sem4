using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FWPS.Models
{
    public class SnapBoxItem : ItemBase
    {

        public string PowerLevel { get; set; }
        public bool MailReceived { get; set; }
        public string ReceiverEmail { get; set; }

    }
}
