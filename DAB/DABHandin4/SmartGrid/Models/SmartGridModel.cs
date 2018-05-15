using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartGrid.Models
{
    public class SmartGridModel : Entity
    {
        [Key]
        public int SmartGridId { get; set; }
        public double TotalForbrug { get; set; }
        public double TotalGeneration { get; set; }
        public double Brutto { get; set; }
    }

    public class SmartGridModelDTO
    {
        public SmartGridModelDTO()
        {

        }

        public SmartGridModelDTO(SmartGridModel smartGridModel)
        {
            SmartGridId = smartGridModel.SmartGridId;
            TotalForbrug = smartGridModel.TotalForbrug;
            TotalGeneration = smartGridModel.TotalGeneration;
            Brutto = smartGridModel.Brutto;
        }

        public int SmartGridId { get; set; }
        public double TotalForbrug { get; set; }
        public double TotalGeneration { get; set; }
        public double Brutto { get; set; }
    }
}