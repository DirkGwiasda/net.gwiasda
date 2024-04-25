namespace Net.Gwiasda.Local.UI.ViewModel.FiMa
{
    public class BookingViewModel
    {
        public BookingViewModel() { }
        public BookingViewModel(Booking booking)
        {
            Id = booking.Id.ToString();
            Timestamp = booking.Timestamp;
            Text = booking.Text;
            CategoryId = booking.CategoryId;
            IsCost = booking.IsCost;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Text { get; set; } = string.Empty;
        public Guid CategoryId { get; set; } = Guid.Empty;
        public bool IsCost { get; set; }

        public Booking ToBooking()
        => new Booking
            {
                Id = Guid.Parse(Id),
                Timestamp = Timestamp,
                Text = Text,
                CategoryId = CategoryId,
                IsCost = IsCost
            };
    }
}