using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FWPS.Models
{
    /////////////////////////////////////////////////
    /// Not used, LazyCurtain item model
    /////////////////////////////////////////////////
    public class CurtainItem : ItemBase
    {
        public string Command { get; set; }
        public bool IsRun { get; set; }
        public int MaxLightIntensity { get; set; }
        public int LightIntensity { get; set; }
        public string Status { get; set; }
    }
}
