namespace Net.Gwiasda.FiMa
{
    public interface IBookingManager
    {
        Task CreateBookingAsync(Booking booking);
    }
}