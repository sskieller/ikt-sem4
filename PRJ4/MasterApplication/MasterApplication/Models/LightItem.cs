﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MasterApplication.Models
{
    public class LightItem : ItemBase
    {
        public string Command { get; set; }
        public bool IsRun { get; set; }
        public DateTime WakeUpTime { get; set; }
        public DateTime SleepTime { get; set; }
        public bool IsOn { get; set; }
    }
}
