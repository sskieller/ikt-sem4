using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Handin33.Models
{
    public class Person
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Email[] Emails { get; set; }
        public PublicAddress[] PublicAddresses { get; set; }
        public PhoneNumber[] PhoneNumbers { get; set; }
    }
}