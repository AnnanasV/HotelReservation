namespace HotelReservation.Models
{
    internal class Room
    {
        public int RoomID { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }

        public int HotelID { get; set; }
        public Hotel Hotel { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

        public Room ()
        {
            Reservations = new List<Reservation>();
        }
    }
}
