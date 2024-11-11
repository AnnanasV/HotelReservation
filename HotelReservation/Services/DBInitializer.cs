using HotelReservation.Models;

namespace HotelReservation.Services
{
    public class DBInitializer
    {
        internal static void Seed(BookingDbContext dbContext)
        {
            if (dbContext.Hotels.Any() || dbContext.Rooms.Any() || dbContext.Reservations.Any())
            {
                return; // Do not need initialization
            }

            var hotels = new[]
            {
                new Hotel { HotelName = "hotel1", Address = "Address 1", Rating = 3 },
                new Hotel { HotelName = "hotel2", Address = "Address 2", Rating = 4 },
                new Hotel { HotelName = "hotel3", Address = "Address 3", Rating = 5 }
            };
            dbContext.Hotels.AddRange(hotels);

            dbContext.SaveChanges();

            var rooms = new[]
            {
                new Room { Type = "room1", Price = 100, HotelID = hotels[0].HotelID },
                new Room { Type = "room2", Price = 150, HotelID = hotels[0].HotelID },
                new Room { Type = "room3", Price = 200, HotelID = hotels[1].HotelID },
                new Room { Type = "room4", Price = 250, HotelID = hotels[1].HotelID },
                new Room { Type = "room5", Price = 300, HotelID = hotels[2].HotelID }
            };
            dbContext.Rooms.AddRange(rooms);

            dbContext.SaveChanges();

            var reservations = new[]
            {
                new Reservation { RoomID = rooms[0].RoomID, StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(3) },
                new Reservation { RoomID = rooms[1].RoomID, StartDate = DateTime.Now.AddDays(4), EndDate = DateTime.Now.AddDays(6) },
                new Reservation { RoomID = rooms[2].RoomID, StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(5) }
            };
            dbContext.Reservations.AddRange(reservations);

            dbContext.SaveChanges();
        }
    }
}
