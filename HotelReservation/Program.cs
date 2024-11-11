using HotelReservation.Models;
using HotelReservation.Services;

public class Program
{
    public static void Main(string[] args)
    {
        using (var dbContext = new BookingDbContext())
        {
            //DBInitializer.Seed(dbContext); // Initialize simple data to db to check its work


        }
    }
}