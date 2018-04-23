namespace Handin32.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PhoneNumbers : Entity
	{
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        public string PhoneType { get; set; }

        public string PhoneCompany { get; set; }

        public int? Person_Id { get; set; }

        public virtual People People { get; set; }
    }

	public class PhoneDto : Entity
	{
		public int Id { get; set; }
		public string PhoneType { get; set; }
		public string PhoneCompany { get; set; }
		public int? PersonId { get; set; }
		public string PersonName { get; set; }
	}
}
