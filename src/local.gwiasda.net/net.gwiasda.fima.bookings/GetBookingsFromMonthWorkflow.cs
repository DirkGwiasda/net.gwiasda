using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa
{
    public class GetBookingsFromMonthWorkflow : IGetBookingsFromMonthWorkflow
    {
        private readonly IBookingManager _bookingManager;

        public GetBookingsFromMonthWorkflow(IBookingManager bookingManager)
        {
            _bookingManager = bookingManager ?? throw new ArgumentNullException(nameof(bookingManager));
        }

        public async Task<List<Booking>> GetBookingsFromMonthAsync(DateTime month)
        {
            var firstDay = new DateTime(month.Year, month.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            
            var result = new List<Booking>();

            var currentDay = firstDay;

            while(currentDay.Date <= lastDay.Date)
            {
                var bookingsFromDay = await _bookingManager.GetBookingsFromDay(currentDay);
                if(bookingsFromDay.Any())
                    result.AddRange(bookingsFromDay);
                currentDay = currentDay.AddDays(1);
            }
            
            return result;
        }
    }
}