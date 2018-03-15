using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FWPS.Models
{
    public class LightItem
    {

        public LightItem()
        {
            DateTime d = DateTime.Now;
            this.CreatedDate = d;
            this.LastModifiedDate = d;
        }
        public long Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Command { get; set; }
        public bool IsRun { get; set; }
    }

	//public class LightContext : DbContext
	//{
	//	public LightContext(DbContextOptions<LightContext> options)
	//		: base(options)
	//	{
	//		//Empty for now
	//	}

	//	public DbSet<MorningSunItem> MorningSunItems { get; set; }
	//}
}
