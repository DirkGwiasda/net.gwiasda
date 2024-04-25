using Net.Gwiasda.FiMa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Net.Gwiasda.Local.Repository
{
    public class FiMaFileSystemBookingRepository : FiMaFileSystemRepository, IBookingRepository
    {
        internal const string BookingDirectory = "booking";
        internal const string BookingFileExtension = ".bookings";

        public async Task CreateBookingAsync(Booking booking)
        {
            if(booking == null) throw new ArgumentNullException(nameof(booking));

            var bookings = (await GetBookingsFromDay(booking.Timestamp)).ToList();
            bookings.Add(booking);
            await WriteBookingsFromDay(bookings);
        }

        public async Task<IEnumerable<Booking>> GetBookingsFromDay(DateTime date)
        {
            var bookingFileName = GetFQBookingFileName(date);
            
            if(!File.Exists(bookingFileName)) return Enumerable.Empty<Booking>();

            var json = await File.ReadAllTextAsync(bookingFileName);
            return JsonSerializer.Deserialize<IEnumerable<Booking>>(json) ?? Enumerable.Empty<Booking>();
        }
        internal async Task WriteBookingsFromDay(IEnumerable<Booking> bookings)
        {
            var json = JsonSerializer.Serialize(bookings);
            var bookingFileName = GetFQBookingFileName(bookings.First().Timestamp);
            await File.WriteAllTextAsync(bookingFileName, json);
        }

        internal string GetFQBookingFileName(DateTime date)
            => Path.Combine(GetBookingMonthDirectory(date), $"{date:yyyy_MM_dd}{BookingFileExtension}");
        internal string GetBookingMonthDirectory(DateTime date)
        {
            var baseDir = base.GetBaseDirectory(BookingDirectory);
            var directory = Path.Combine(baseDir, $"{date:yyyy_MM}");
            if(!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            return directory;
        }
    }
}
