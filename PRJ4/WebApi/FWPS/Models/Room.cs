using System.ComponentModel.DataAnnotations;

namespace FWPS.Models
{
    /////////////////////////////////////////////////
    /// Room item model for Poomba
    /////////////////////////////////////////////////
    public class Room
    {
        [Key]
        public string RoomName { get; set; } 
    }
}