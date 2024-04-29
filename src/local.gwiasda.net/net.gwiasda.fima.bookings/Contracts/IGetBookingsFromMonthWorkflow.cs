namespace Net.Gwiasda.FiMa
{
    public interface IGetBookingsFromMonthWorkflow
    {
        Task<List<Booking>> GetBookingsFromMonthAsync(DateTime month);
    }
}