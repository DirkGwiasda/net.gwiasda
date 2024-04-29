using Microsoft.AspNetCore.Mvc;
using Net.Gwiasda.FiMa;
using Net.Gwiasda.Local.UI.ViewModel.FiMa;
using Net.Gwiasda.Logging;
using System.Globalization;

namespace Net.Gwiasda.Local.UI.Controllers
{
    public class FiMaReportController : Controller
    {
        private const string APP_NAME = "FiMa";
        private readonly ILoggingManager _loggingManager;
        private readonly ICreateMonthlyReportWorkflow _createMonthlyReportWorkflow;

        public FiMaReportController(ILoggingManager loggingManager, ICreateMonthlyReportWorkflow createMonthlyReportWorkflow)
        {
            _loggingManager = loggingManager ?? throw new ArgumentNullException(nameof(loggingManager));
            _createMonthlyReportWorkflow = createMonthlyReportWorkflow ?? throw new ArgumentNullException(nameof(createMonthlyReportWorkflow));

        }

        public Task<string> Ping()
        {
            return Task.FromResult($"{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")} Pong from FiMaReportController");
        }

        public async Task<MonthlyReportViewModel> GetMonthlyReport(string date)
        {
            try
            {
                if(!DateTime.TryParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var day))
                {
                    throw new ArgumentException($"Invalid date format '{date}'", nameof(date));
                }
                var monthlyReport = await _createMonthlyReportWorkflow.CreateMonthlyReportAsync(day);

                return new MonthlyReportViewModel(monthlyReport);
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc);
                throw;
            }
        }
    }
}