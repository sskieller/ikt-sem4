using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FWPS.Models
{
    public class ClimateControlItem : ItemBase
    {
        public string Command { get; set; }
        public bool IsRun { get; set; }
        public int TemperatureLevel { get; set; }
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }
        public int HumidityLevel { get; set; }
        public int MaxHumidity { get; set; }
        public int MinHumidity { get; set; }
        public bool IsVentilationOn { get; set; }
        public bool IsHeaterOn { get; set; }
    
    }
    
}
