using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FWPS.Models
{
    public class MailItem : ItemBase
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
