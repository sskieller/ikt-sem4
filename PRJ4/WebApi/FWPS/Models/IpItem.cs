using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FWPS.Models
{
    /////////////////////////////////////////////////
    /// IP item model
    /////////////////////////////////////////////////
    public class IpItem : ItemBase
    {

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
