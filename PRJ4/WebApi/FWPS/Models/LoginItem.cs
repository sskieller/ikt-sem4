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

        public string Username { get; set; }

        public string Password { get; set; }
        public string RfidId1 { get; set; }
        public string RfidId2 { get; set; }
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
