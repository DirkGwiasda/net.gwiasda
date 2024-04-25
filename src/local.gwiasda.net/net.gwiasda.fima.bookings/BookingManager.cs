namespace Net.Gwiasda.FiMa
{
    public class BookingManager : IBookingManager
    {
        private readonly IBookingRepository _repository;
        public BookingManager(IBookingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task CreateBookingAsync(Booking booking)
         => await _repository.CreateBookingAsync(booking);
    }
}