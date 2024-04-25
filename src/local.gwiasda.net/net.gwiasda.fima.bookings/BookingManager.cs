namespace Net.Gwiasda.FiMa
{
    public class BookingManager : IBookingManager
    {
        private readonly IBookingRepository _repository;
        public BookingManager(IBookingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task CreateOrUpdateBookingAsync(Booking booking)
        {
            if (booking == null) throw new ArgumentNullException(nameof(booking));

            await _repository.DeleteBookingIfExistsAsync(booking);
            await _repository.CreateBookingAsync(booking);
        }

        public async Task DeleteBookingAsync(Booking booking)
        {
            if(booking == null) throw new ArgumentNullException(nameof(booking));
            await _repository.DeleteBookingIfExistsAsync(booking);
        }

        public async Task<IEnumerable<Booking>> GetBookingsFromDay(DateTime date)
         => (await _repository.GetBookingsFromDay(date)).ToList();
    }
}