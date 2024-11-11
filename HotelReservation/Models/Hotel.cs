using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Models
{
    internal class Hotel
    {
        public int HotelID { get; set; }
        public string HotelName { get; set; }
        public string Address { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public ICollection<Room> Rooms { get; set; }

        public Hotel()
        {
            Rooms = new List<Room>();
        }
    }
}
