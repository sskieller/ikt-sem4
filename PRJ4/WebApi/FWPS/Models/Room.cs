using System.ComponentModel.DataAnnotations;

namespace FWPS.Models
{
    public class Room
    {
        [Key]
        public string RoomName { get; set; } 
    }
}