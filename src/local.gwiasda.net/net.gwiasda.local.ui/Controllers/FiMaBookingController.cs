using Microsoft.AspNetCore.Mvc;
using Net.Gwiasda.FiMa;
using Net.Gwiasda.Local.UI.ViewModel.FiMa;
using Net.Gwiasda.Logging;

namespace Net.Gwiasda.Local.UI.Controllers
{
    public class FiMaBookingController : Controller
    {
        private const string APP_NAME = "FiMa";
        private readonly ILoggingManager _loggingManager;
        private readonly IBookingManager _bookingManager;

        public FiMaBookingController(ILoggingManager loggingManager, IBookingManager bookingManager)
        {
            _loggingManager = loggingManager ?? throw new ArgumentNullException(nameof(loggingManager));
            _bookingManager = bookingManager ?? throw new ArgumentNullException(nameof(bookingManager));
        }

        public Task<string> Ping()
        {
            return Task.FromResult($"{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")} Pong from FiMaBookingController");
        }

        [HttpPost]
        public async Task Save([FromBody] BookingViewModel bookingViewModel)
        {
            try
            {
                if (bookingViewModel == null) throw new ArgumentNullException(nameof(bookingViewModel));
                var booking = bookingViewModel.ToBooking();

                await _bookingManager.CreateBookingAsync(booking).ConfigureAwait(true);
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc).ConfigureAwait(true);
                throw;
            }
        }
    }
}