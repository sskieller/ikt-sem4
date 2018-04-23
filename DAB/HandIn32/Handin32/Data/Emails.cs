namespace Handin32.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Emails : Entity
    {
        public int Id { get; set; }

        [Required]
        public string MailAddress { get; set; }

        public int? Person_Id { get; set; }

        public virtual People People { get; set; }
    }

	public class EmailDto : Entity
	{
		public int Id { get; set; }
		public string MailAddress { get; set; }
		public int PersonId { get; set; }
	}
}
