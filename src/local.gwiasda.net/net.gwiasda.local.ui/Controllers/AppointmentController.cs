//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Server.IIS.Core;
//using Net.Gwiasda.Appointments;
//using Net.Gwiasda.Local.UI.ViewModel.Appointments;
//using Net.Gwiasda.Local.UI.ViewModel.Logging;
//using System.Collections.ObjectModel;

//namespace Net.Gwiasda.Local.UI.Controllers
//{
//    public class AppointmentController : Controller
//    {
//        private readonly ILogService _logService;
//        private readonly IAppointmentService _appointmentService;

//        public AppointmentController(ILogService logger, IAppointmentService appointmentService)
//        {
//            _logService = logger ?? throw new ArgumentNullException(nameof(logger));
//            _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
//        }

//        public Task<string> Ping() => Task.FromResult($"{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")} Pong from AppointmentController");

//        public async Task<ICollection<AppointmentViewModel>> GetAll()
//        {
//            try
//            {
//                return (await _appointmentService.GetAllAsync().ConfigureAwait(true)).Select(a => new AppointmentViewModel(a)).ToList();
//            }
//            catch(Exception exc)
//            {
//                await _logService.CreateErrorAsync(LogApplication.Appointments, exc).ConfigureAwait(true);
//                throw;
//            }
//        }

//        [HttpPost]
//        public async Task Save([FromBody] AppointmentViewModel appointmentViewModel)
//        {
//            try
//            {
//                if (appointmentViewModel == null) throw new ArgumentNullException(nameof(appointmentViewModel));

//                var appointment = appointmentViewModel.ToAppointment();
//                await _appointmentService.SaveAsync(appointment).ConfigureAwait(true);
//            }
//            catch (Exception exc)
//            {
//                await _logService.CreateErrorAsync(LogApplication.Appointments, exc).ConfigureAwait(true);
//                throw;
//            }
//        }
//        public async Task Delete(string? id)
//        {
//            try
//            {
//                await _appointmentService.DeleteAsync(id).ConfigureAwait(true);
//            }
//            catch (Exception exc)
//            {
//                await _logService.CreateErrorAsync(LogApplication.Appointments, exc).ConfigureAwait(true);
//                throw;
//            }
//        }
//    }
//}