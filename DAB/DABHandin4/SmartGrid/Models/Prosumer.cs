using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartGrid.Models
{
    public class Prosumer : Entity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public float Produced { get; set; }
        public float Consumed { get; set; }
    }
}