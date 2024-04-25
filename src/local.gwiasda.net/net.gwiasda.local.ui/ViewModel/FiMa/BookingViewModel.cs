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
            CategoryId = booking.CategoryId.ToString();
            IsCost = booking.IsCost;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Text { get; set; } = string.Empty;
        public string CategoryId { get; set; } = Guid.NewGuid().ToString();
        public bool IsCost { get; set; }
        public decimal Amount { get; set; }

        public Booking ToBooking()
        => new Booking
            {
                Id = Guid.Parse(Id),
                Timestamp = Timestamp,
                Text = Text,
                CategoryId = Guid.Parse(CategoryId),
                IsCost = IsCost
            };
    }
}