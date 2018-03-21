using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FWPS.Models
{
    public class LoginItem : ItemBase
    {
        public LoginItem()
        {
            DateTime d = DateTime.Now;
            this.CreatedDate = d;
            this.LastModifiedDate = d;
        }
        [Key]
        public string Username { get; set; }

        public string Password { get; set; }
    }

    //public class LoginContext : DbContext
    //{
    //    public LoginContext(DbContextOptions<LoginContext> options)
    //        : base(options)
    //    {
    //        //Empty for now
    //    }

    //    public DbSet<LoginItem> LoginItems { get; set; }
    //}


}
