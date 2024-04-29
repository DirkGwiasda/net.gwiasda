namespace Net.Gwiasda.FiMa
{
    public interface ICreateMonthlyReportWorkflow
    {
        Task<MonthlyReport> CreateMonthlyReportAsync(DateTime month);
    }
}