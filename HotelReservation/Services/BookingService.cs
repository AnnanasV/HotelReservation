using HotelReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Services
{
    internal class BookingService
    {
        private readonly BookingDbContext _dbContext;

        internal BookingService(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Method to check room availability
        private async Task<bool> IsRoomAvailableAsync(int roomId, DateTime startDate, DateTime endDate)
        {
            return !await _dbContext.Reservations
                    .AnyAsync(r => r.RoomID == roomId &&
                        ((startDate < r.EndDate && startDate >= r.StartDate) ||
                        (endDate > r.StartDate && endDate <= r.EndDate) ||
                        (startDate <= r.StartDate && endDate >= r.EndDate)));
        }

        public async Task<Reservation> CreateReservationAsync(int roomId, DateTime startDate, DateTime endDate)
        {
            var isRoomAvailable = await IsRoomAvailableAsync(roomId, startDate, endDate);
            
            if (!isRoomAvailable)
            {
                throw new InvalidOperationException("Room is not available for the selected dates.");
            }

            var reservation = new Reservation
            {
                RoomID = roomId,
                StartDate = startDate,
                EndDate = endDate
            };

            _dbContext.Reservations.Add(reservation);
            await _dbContext.SaveChangesAsync();

            return reservation;
        }

        public async Task<Reservation> UpdateReservationAsync(int reservationId, DateTime startDate, DateTime endDate)
        {
            // Find reservation
            var reservation = await _dbContext.Reservations.FindAsync(reservationId);

            if (reservation == null)
            {
                throw new InvalidOperationException("Reservation not found.");
            }

            // Check if new date is available
            var isRoomAvailable = await IsRoomAvailableAsync(reservation.RoomID, startDate, endDate);

            if (!isRoomAvailable)
            {
                throw new InvalidOperationException("Room is not available for the new dates.");
            }

            reservation.StartDate = startDate;
            reservation.EndDate = endDate;
            await _dbContext.SaveChangesAsync();
            return reservation;
        }

        public async Task DeleteReservationAsync(int reservationId)
        {
            var reservation = await _dbContext.Reservations.FindAsync(reservationId);
            if (reservation == null)
            {
                throw new KeyNotFoundException("Reservation not found.");
            }

            _dbContext.Reservations.Remove(reservation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Reservation>> GetReservationsAsync(int roomId)
        {
            var reservations = await _dbContext.Reservations
                .Where(r => r.RoomID == roomId)
                .ToListAsync();

            return reservations;
        }

        public async Task<List<Room>> GetRoomsAsync(DateTime startDate, DateTime endDate)
        {
            var reservedRooms = await _dbContext.Reservations
                .Where(r => (startDate >= r.StartDate && startDate < r.EndDate) ||
                            (endDate > r.StartDate && endDate <= r.EndDate) ||
                            (startDate <= r.StartDate && endDate >= r.EndDate))
                .Select(r => r.RoomID)
                .ToListAsync();

            var availableRooms = await _dbContext.Rooms
                .Where(r => !reservedRooms.Contains(r.RoomID))
                .Include(r => r.Hotel)
                .ToListAsync();

            return availableRooms;
        }
    }
}
