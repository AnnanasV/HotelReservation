using HotelReservation.Models;
using HotelReservation.Services;
using Microsoft.IdentityModel.Tokens;

public class Program
{
    public static void Main(string[] args)
    {
        using (var dbContext = new BookingDbContext())
        {
            //DBInitializer.Seed(dbContext); // Initialize simple data to db to check its work

            var bookingService = new BookingService(dbContext);

            RunMenuAsync(bookingService).GetAwaiter().GetResult(); // Cannot set Main() async

        }
    }

    private static async Task RunMenuAsync(BookingService bookingService)
    {
        DateTime startDate = DateTime.MinValue, endDate = DateTime.MinValue;
        int roomId = 0;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n--- Booking service ---");
            Console.WriteLine("1. Available rooms");
            Console.WriteLine("2. Create Reservation");
            Console.WriteLine("3. Delete reservation");
            Console.WriteLine("4. Edit reservation");
            Console.WriteLine("0. Exit");
            Console.WriteLine("\n");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    GetDate(ref startDate, ref endDate);

                    var availableRooms = await bookingService.GetRoomsAsync(startDate, endDate);

                    Console.WriteLine("\nAvailable rooms:");
                    foreach (var room in availableRooms)
                    {
                        Console.WriteLine($"Hotel: {room.Hotel.HotelName}\tRoom Id: {room.RoomID}\tType: {room.Type}\tPrice: {room.Price}");
                    }
                    break;
                case "2":
                    GetDate(ref startDate, ref endDate);
                    GetRoomId(ref roomId);

                    await bookingService.CreateReservationAsync(roomId, startDate, endDate);
                    Console.WriteLine("Success");
                    break;
                case "3":
                    await DeleteReservation(bookingService);
                    Console.WriteLine("Success");
                    break;
                case "4":
                    GetDate(ref startDate, ref endDate);
                    GetRoomId(ref roomId);

                    await bookingService.UpdateReservationAsync(roomId, startDate, endDate);
                    Console.WriteLine("Success");
                    break;
                case "0":
                    Console.WriteLine("Exit");
                    return;
                default:
                    Console.WriteLine("NaN");
                    break;
            }
            Console.ReadKey();
        }
    }

    private static async Task DeleteReservation(BookingService bookingService)
    {
        int roomId = 0;
        GetRoomId(ref roomId);

        var reservations = await bookingService.GetReservationsAsync(roomId);

        Console.WriteLine("Reservations:");
        foreach (var reservation in reservations)
        {
            Console.WriteLine($"Reservation Id: {reservation.ReservationID}\tRoom Id: {reservation.RoomID}\tStart: {reservation.StartDate}\tEnd: {reservation.EndDate}");
        }

        Console.WriteLine("Enter reservation Id:");
        int id;
        Int32.TryParse(Console.ReadLine(), out id);

        await bookingService.DeleteReservationAsync(id);
    }

    private static void GetRoomId(ref int roomId)
    {
        Console.WriteLine("Enter room id");
        Int32.TryParse(Console.ReadLine(), out roomId);
    }

    private static void GetDate(ref DateTime startDate, ref DateTime endDate)
    {
        // if user want to leave date
        if (!startDate.Equals(DateTime.MinValue) && !endDate.Equals(DateTime.MinValue))
        {
            Console.WriteLine($"Leave {startDate.Date} - {endDate.Date}?\n1. Yes\n2. No");
            if(Console.ReadLine() == "1") 
                return;
        }

        bool isStartOk, isEndOk;
        do
        {
            Console.WriteLine("Enter start end end date:");

            isStartOk = DateTime.TryParse(Console.ReadLine(), out startDate);
            isEndOk = DateTime.TryParse(Console.ReadLine(), out endDate);
        } while (!isEndOk && !isStartOk);
    }
}