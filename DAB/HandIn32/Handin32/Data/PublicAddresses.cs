namespace Handin32.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PublicAddresses : Entity
	{
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PublicAddresses()
        {
            People = new HashSet<People>();
        }

        public int Id { get; set; }

        [Required]
        public string AddressType { get; set; }

        [Required]
        public string StreetName { get; set; }

        [Required]
        public string HouseNumber { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string City { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<People> People { get; set; }
    }

	public class PublicAddressDto : Entity
	{
		public int Id { get; set; }

		[Required]
		public string StreetName { get; set; }

		[Required]
		public string HouseNumber { get; set; }
	}

	public class PublicAddressDetailDto : Entity
	{

		public int Id { get; set; }

		public string AddressType { get; set; }

		[Required]
		public string StreetName { get; set; }

		[Required]
		public string HouseNumber { get; set; }

		[Required]
		public string ZipCode { get; set; }

		[Required]
		public string City { get; set; }

	    public IEnumerable<string> People;
	}
}
