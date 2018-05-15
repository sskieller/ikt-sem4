using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SmartGrid.Models
{
    public class Prosumer : Entity
    {
        [Key]
        public string Name { get; set; }
        [ForeignKey("Name")]
        public virtual Prosumer PreferedBuyer { get; set; }
        public float Produced { get; set; }
        public float Consumed { get; set; }
        public float Difference { get; set; }

    }

    public class ProsumerDTO
    {
        public ProsumerDTO()
        { }

        public ProsumerDTO(Prosumer pro)
        {
            Name = pro.Name;
            PreferedBuyer = pro.PreferedBuyer.Name == null ? pro.PreferedBuyer.Name : string.Empty;
            Produced = pro.Produced;
            Consumed = pro.Consumed;
        }

        public string Name { get; set; }
        public string PreferedBuyer { get; set; }
        public float Produced { get; set; }
        public float Consumed { get; set; }
    }
}