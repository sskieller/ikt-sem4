namespace Handin32.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class People : Entity
	{
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public People()
        {
            Emails = new HashSet<Emails>();
            PhoneNumbers = new HashSet<PhoneNumbers>();
            PublicAddresses = new HashSet<PublicAddresses>();
        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Emails> Emails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhoneNumbers> PhoneNumbers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PublicAddresses> PublicAddresses { get; set; }
    }

	public class PeopleDto : Entity
	{
		public int Id { get; set; }
		public string Firstname { get; set; }
		public string LastName { get; set; }
		public virtual IEnumerable<EmailDto> Emails { get; set; }
		public virtual IEnumerable<PhoneDto> PhoneNumbers { get; set; }
		public virtual IEnumerable<PublicAddressDto> PublicAddresses { get; set; }
	}

	public class PeopleDetailDto : Entity
	{
		public int Id { get; set; }
		public string Firstname { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public virtual ICollection<EmailDto> Emails { get; set; }
		public virtual ICollection<PhoneDto> PhoneNumbers { get; set; }
		public virtual ICollection<PublicAddressDetailDto> PublicAddresses { get; set; }
	}
}
