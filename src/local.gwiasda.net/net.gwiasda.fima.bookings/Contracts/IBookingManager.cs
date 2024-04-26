namespace Net.Gwiasda.FiMa
{
    public interface IBookingManager
    {
        Task CreateOrUpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(Booking booking);
        Task<IEnumerable<Booking>> GetBookingsFromDay(DateTime date);
        Task CreateOrUpdateRecurringBookingAsync(RecurringBooking recurringBooking);
        Task DeleteRecurringBookingAsync(RecurringBooking recurringBooking);
        Task<IEnumerable<RecurringBooking>> GetRecurringBookings();
    }
}