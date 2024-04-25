namespace Net.Gwiasda.FiMa
{
    public interface IBookingManager
    {
        Task CreateBookingAsync(Booking booking);
        Task<IEnumerable<Booking>> GetBookingsFromDay(DateTime date);
    }
}