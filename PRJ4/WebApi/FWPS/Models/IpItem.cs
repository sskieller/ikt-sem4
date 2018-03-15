using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FWPS.Models
{
    public class IpItem
    {
	    public IpItem()
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
	    public string Ip { get; set; }
	}

	//public class IpContext : DbContext
	//{
	//	public IpContext(DbContextOptions<IpContext> options)
	//		: base(options)
	//	{
	//		//Empty for now
	//	}

	//	public DbSet<IpItem> IpItems { get; set; }
	//}


}
