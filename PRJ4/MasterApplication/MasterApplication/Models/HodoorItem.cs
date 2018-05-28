using System;
using System.Collections.Generic;
using System.Text;

namespace MasterApplication.Models
{
    /////////////////////////////////////////////////
    /// Item representing the HodoorItem on the WebApi
    /////////////////////////////////////////////////
    public class HodoorItem : ItemBase
    {
        public string Command { get; set; }
        public bool OpenStatus { get; set; }
        public bool IsRun { get; set; }
        
    }
}
