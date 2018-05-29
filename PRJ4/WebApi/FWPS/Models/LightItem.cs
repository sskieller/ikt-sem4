using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FWPS.Models
{
    /////////////////////////////////////////////////
    /// MorningSun item model
    /////////////////////////////////////////////////
    public class LightItem : ItemBase
    {

        public string Command { get; set; }
        public bool IsRun { get; set; }
        public DateTime WakeUpTime { get; set; }
        public DateTime SleepTime { get; set; }
        public bool IsOn { get; set; }

    }
}
