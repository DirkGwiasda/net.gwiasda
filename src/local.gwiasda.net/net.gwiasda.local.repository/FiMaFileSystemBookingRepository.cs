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
            var bookings = (await GetBookingsFromDay(booking.Timestamp)).ToList();
            bookings.Add(booking);
            await WriteBookingsFromDay(bookings);
        }
        public async Task DeleteBookingIfExistsAsync(Booking booking)
        {
            var bookings = (await GetBookingsFromDay(booking.Timestamp)).ToList();
            var item = bookings.FirstOrDefault(b => b.Id == booking.Id);
            if (item != null)
            {
                bookings.Remove(item);
                await WriteBookingsFromDay(bookings);
            }
        }

        public async Task<IEnumerable<Booking>> GetBookingsFromDay(DateTime date)
        {
            var bookingFileName = GetFQBookingFileName(date, false);
            
            if(!File.Exists(bookingFileName)) return Enumerable.Empty<Booking>();

            var json = await File.ReadAllTextAsync(bookingFileName);
            return JsonSerializer.Deserialize<IEnumerable<Booking>>(json) ?? Enumerable.Empty<Booking>();
        }

        public async Task CreateRecurringBookingAsync(RecurringBooking recurringBooking)
        {
            var recurringBookings = (await GetRecurringBookings()).ToList();
            recurringBookings.Add(recurringBooking);
            await WriteRecurringBookings(recurringBookings);
        }
        public async Task DeleteRecurringBookingIfExistsAsync(RecurringBooking recurringBooking)
        {
            var recurringBookings = (await GetRecurringBookings()).ToList();
            var item = recurringBookings.FirstOrDefault(b => b.Id == recurringBooking.Id);
            if (item != null)
            {
                recurringBookings.Remove(item);
                await WriteRecurringBookings(recurringBookings);
            }
        }
        public async Task<IEnumerable<RecurringBooking>> GetRecurringBookings()
        {
            var fileName = GetFQRecurringBookingFileName();

            if (!File.Exists(fileName)) return Enumerable.Empty<RecurringBooking>();

            var json = await File.ReadAllTextAsync(fileName);
            return JsonSerializer.Deserialize<IEnumerable<RecurringBooking>>(json) ?? Enumerable.Empty<RecurringBooking>();
        }

        internal async Task WriteBookingsFromDay(IEnumerable<Booking> bookings)
        {
            var json = JsonSerializer.Serialize(bookings);
            var bookingFileName = GetFQBookingFileName(bookings.First().Timestamp, true);
            await File.WriteAllTextAsync(bookingFileName, json);
        }
        internal async Task WriteRecurringBookings(IEnumerable<RecurringBooking> bookings)
        {
            var json = JsonSerializer.Serialize(bookings);
            var fileName = GetFQRecurringBookingFileName();
            await File.WriteAllTextAsync(fileName, json);
        }

        internal string GetFQRecurringBookingFileName()
            => Path.Combine(base.GetBaseDirectory(BookingDirectory), $"recurring{BookingFileExtension}");
        internal string GetFQBookingFileName(DateTime date, bool createDirectoryIfNotExists)
            => Path.Combine(GetBookingMonthDirectory(date, createDirectoryIfNotExists), $"{date:yyyy_MM_dd}{BookingFileExtension}");
        internal string GetBookingMonthDirectory(DateTime date, bool createIfNotExists)
        {
            var baseDir = base.GetBaseDirectory(BookingDirectory);
            var directory = Path.Combine(baseDir, $"{date:yyyy_MM}");
            if(createIfNotExists && !Directory.Exists(directory)) Directory.CreateDirectory(directory);
            return directory;
        }
    }
}
