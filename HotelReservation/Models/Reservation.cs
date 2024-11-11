namespace HotelReservation.Models
{
    internal class Reservation
    {
        public int ReservationID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int RoomID { get; set; }
        public Room Room { get; set; }
    }
}
