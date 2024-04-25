namespace Net.Gwiasda.FiMa
{
    public interface IBookingManager
    {
        Task CreateOrUpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(Booking booking);
        Task<IEnumerable<Booking>> GetBookingsFromDay(DateTime date);
    }
}