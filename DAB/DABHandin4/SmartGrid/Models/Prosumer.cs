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
        [ForeignKey("PreferedBuyer")]
        public virtual Prosumer PreferedBuyer { get; set; }
        public float Produced { get; set; }
        public float Consumed { get; set; }
        public float Differece { get; set; }
    }
}