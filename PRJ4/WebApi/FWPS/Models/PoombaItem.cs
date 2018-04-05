using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FWPS.Models
{
    public class PoombaItem : ItemBase
    {
        public string Command { get; set; }
        public bool IsRun { get; set; }
        public List<string> RoomsToClean { get; set; }
        public DateTime CleaningTime { get; set; }

    }
}
