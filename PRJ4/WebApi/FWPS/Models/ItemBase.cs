using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace FWPS.Models
{
    public abstract class ItemBase
    {
        protected ItemBase()
        {
            DateTime d = DateTime.Now;
            this.CreatedDate = d;
            this.LastModifiedDate = d;
        }
        [Key]
        public long Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }
}
}
