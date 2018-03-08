using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FWPS.Models
{
    public class LoginItem
    {
        public LoginItem()
        {
            DateTime d = DateTime.Now;
            this.CreatedDate = d;
            this.LastModifiedDate = d;
        }
        public string Username { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Password { get; set; }
    }

    public class LoginContext : DbContext
    {
        public LoginContext(DbContextOptions<LoginContext> options)
            : base(options)
        {
            //Empty for now
        }

        public DbSet<LoginItem> LoginItems { get; set; }
    }


}
