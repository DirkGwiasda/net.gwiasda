using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa
{
    public interface IBookingRepository
    {
        Task CreateBookingAsync(Booking booking);
        Task DeleteBookingIfExistsAsync(Booking booking);
        Task<IEnumerable<Booking>> GetBookingsFromDay(DateTime date);
    }
}