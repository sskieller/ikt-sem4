using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FWPS.Models
{
    public class HodoorItem : ItemBase
    {
        public string Command { get; set; }
        public bool OpenStatus { get; set; }
        public bool IsRun { get; set; }
    }
}
