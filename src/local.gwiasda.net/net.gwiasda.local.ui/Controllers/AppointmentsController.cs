using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Net.Gwiasda.Appointments;
using Net.Gwiasda.FiMa;
using Net.Gwiasda.Links;
using Net.Gwiasda.Local.UI.ViewModel;
using Net.Gwiasda.Local.UI.ViewModel.FiMa;
using Net.Gwiasda.Logging;
using System.Globalization;

namespace Net.Gwiasda.Local.UI.Controllers
{
    public class AppointmentsController : Controller
    {
        private const string APP_NAME = "Appointments";
        private readonly ILoggingManager _loggingManager;
        private readonly IAppointmentManager _appointmentManager;
        private readonly IGetAppointmentsForTimespanWorkflow _getAppointmentsForTimespanWorkflow;

        public AppointmentsController(ILoggingManager loggingManager, IAppointmentManager appointmentManager, IGetAppointmentsForTimespanWorkflow getAppointmentsForTimespanWorkflow)
        {
            _loggingManager = loggingManager ?? throw new ArgumentNullException(nameof(loggingManager));
            _appointmentManager = appointmentManager ?? throw new ArgumentNullException(nameof(appointmentManager));
            _getAppointmentsForTimespanWorkflow = getAppointmentsForTimespanWorkflow ?? throw new ArgumentNullException(nameof(getAppointmentsForTimespanWorkflow));
        }

        public Task<string> Ping()
        {
            return Task.FromResult($"{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")} Pong from AppointmentsController");
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetAppointmentsForTimespan(string from, string to)
        {
            try
            {
                var fromDate = ParseDate(from);
                var toDate = ParseDate(to);

                return (await _getAppointmentsForTimespanWorkflow.GetAppointmentsForTimespanAsync(fromDate, toDate))
                    .Select(a => new AppointmentViewModel(a));
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc);
                throw;
            }
        }
        public async Task<AppointmentViewModel> GetAppointment(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid uid))
                    throw new ArgumentException($"Invalid id format '{id}'", nameof(id));

                return new AppointmentViewModel(await _appointmentManager.GetAppointmentByIdAsync(uid));
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc);
                throw;
            }
        }
        [HttpPost]
        public async Task Save([FromBody] AppointmentViewModel appointmentViewModel)
        {
            try
            {
                if (appointmentViewModel == null) throw new ArgumentNullException(nameof(appointmentViewModel));
                var appointment = appointmentViewModel.ToAppointment();

                await _appointmentManager.CreateOrUpdateAppointmentAsync(appointment);
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc);
                throw;
            }
        }
        public async Task Delete(string? id)
        {
            try
            {
                if(!Guid.TryParse(id, out Guid uid))
                    throw new InvalidCastException($"Cannot parse link id '{id}'.");

                await _appointmentManager.DeleteAppointmentIfExistsAsync(uid);
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc).ConfigureAwait(true);
                throw;
            }
        }
        protected DateTime ParseDate(string dateTime)
        {
            const string DATE_FORMAT = "yyyyMMdd";

            if (!DateTime.TryParseExact(dateTime, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                throw new ArgumentException($"Invalid date format '{dateTime}'.", nameof(dateTime));

            return result;
        }
    }
}