using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Net.Gwiasda.FiMa;
using Net.Gwiasda.Local.UI.ViewModel.FiMa;
using Net.Gwiasda.Local.UI.ViewModel.Logging;
using Net.Gwiasda.Logging;
using System.Collections.ObjectModel;

namespace Net.Gwiasda.Local.UI.Controllers
{
    public class FiMaController : Controller
    {
        private const string APP_NAME = "FiMa";
        private readonly ILoggingManager _loggingManager;
        private readonly ICategoryManager _categoryManager;

        public FiMaController(ILoggingManager loggingManager, ICategoryManager categoryManager)
        {
            _loggingManager = loggingManager ?? throw new ArgumentNullException(nameof(loggingManager));
            _categoryManager = categoryManager ?? throw new ArgumentNullException(nameof(categoryManager));
        }

        public Task<string> Ping()
        {
            return Task.FromResult($"{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")} Pong from FiMaController");
        }

        public async Task<IEnumerable<FinanceCategoryViewModel>> GetCostCategories()
        {
            try
            {
                return (await _categoryManager.GetCategoriesAsync<CostCategory>())
                    .Select(a => new FinanceCategoryViewModel(a)).ToList();
            }
            catch (Exception exc)
            {
                await _loggingManager.InsertErrorAsync(APP_NAME, exc);
                throw;
            }
        }

        //[HttpPost]
        //public async Task Save([FromBody] AppointmentViewModel appointmentViewModel)
        //{
        //    try
        //    {
        //        if (appointmentViewModel == null) throw new ArgumentNullException(nameof(appointmentViewModel));

        //        var appointment = appointmentViewModel.ToAppointment();
        //        await _appointmentService.SaveAsync(appointment).ConfigureAwait(true);
        //    }
        //    catch (Exception exc)
        //    {
        //        await _logService.CreateErrorAsync(LogApplication.Appointments, exc).ConfigureAwait(true);
        //        throw;
        //    }
        //}
        //public async Task Delete(string? id)
        //{
        //    try
        //    {
        //        await _appointmentService.DeleteAsync(id).ConfigureAwait(true);
        //    }
        //    catch (Exception exc)
        //    {
        //        await _logService.CreateErrorAsync(LogApplication.Appointments, exc).ConfigureAwait(true);
        //        throw;
        //    }
        //}
    }
}