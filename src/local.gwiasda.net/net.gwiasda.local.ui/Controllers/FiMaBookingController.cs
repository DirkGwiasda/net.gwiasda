using Microsoft.AspNetCore.Mvc;
using Net.Gwiasda.FiMa;
using Net.Gwiasda.Local.UI.ViewModel.FiMa;
using Net.Gwiasda.Logging;
using System.Globalization;

namespace Net.Gwiasda.Local.UI.Controllers
{
    public class FiMaBookingController : Controller
    {
        private const string APP_NAME = "FiMa";
        private readonly ILoggingManager _loggingManager;
        private readonly IBookingManager _bookingManager;
        private readonly IGetBookingsFromDateWorkflow _getBookingsFromDayWorkflow;

        public FiMaBookingController(ILoggingManager loggingManager, IBookingManager bookingManager, 
                                     IGetBookingsFromDateWorkflow getBookingsFromDayWorkflow)
        {
            _loggingManager = loggingManager ?? throw new ArgumentNullException(nameof(loggingManager));
            _bookingManager = bookingManager ?? throw new ArgumentNullException(nameof(bookingManager));
            _getBookingsFromDayWorkflow = getBookingsFromDayWorkflow ?? throw new ArgumentNullException(nameof(getBookingsFromDayWorkflow));

        }

        public Task<string> Ping()
        {
            return Task.FromResult($"{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")} Pong from FiMaBookingController");
        }

        public async Task<Dictionary<string, List<BookingViewModel>>> GetBookingsFromToday(string date)
        {
            try
            {
                if(!DateTime.TryParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var day))
                {
                    throw new ArgumentException($"Invalid date format '{date}'", nameof(date));
                }
                var bookings = await _getBookingsFromDayWorkflow.GetBookingsFromDay(day);
                var result = new Dictionary<string, List<BookingViewModel>>();
                foreach(var key in bookings.Keys)
                {
                    result.Add(key, bookings[key].Select(b => new BookingViewModel(b)).ToList());
                }

                return result;
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc);
                throw;
            }
        }

        [HttpPost]
        public async Task Save([FromBody] BookingViewModel bookingViewModel)
        {
            try
            {
                if (bookingViewModel == null) throw new ArgumentNullException(nameof(bookingViewModel));
                var booking = bookingViewModel.ToBooking();

                await _bookingManager.CreateOrUpdateBookingAsync(booking).ConfigureAwait(true);
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc).ConfigureAwait(true);
                throw;
            }
        }
    }
}