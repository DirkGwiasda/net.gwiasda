using Net.Gwiasda.FiMa;

namespace Net.Gwiasda.Local.UI.ViewModel.FiMa
{
    public class BookingViewModel
    {
        internal static Dictionary<RecurringType, string> RecurringTypes = new Dictionary<RecurringType, string>
        {
            { Net.Gwiasda.FiMa.RecurringType.None, "einmalig" },
            { Net.Gwiasda.FiMa.RecurringType.Once, "einmalig" },
            { Net.Gwiasda.FiMa.RecurringType.Monthly, "monatlich" },
            { Net.Gwiasda.FiMa.RecurringType.Yearly, "jährlich" }
        };

        public BookingViewModel() { }
        public BookingViewModel(Booking booking)
        {
            Id = booking.Id.ToString();
            Timestamp = booking.Timestamp;
            Text = booking.Text;
            CategoryId = booking.CategoryId.ToString();
            IsCost = booking.IsCost;
            Amount = booking.Amount;
        }
        public BookingViewModel(RecurringBooking recurringBooking)
        {
            Id = recurringBooking.Id.ToString();
            Timestamp = recurringBooking.Timestamp;
            Text = recurringBooking.Text;
            CategoryId = recurringBooking.CategoryId.ToString();
            IsCost = recurringBooking.IsCost;
            Amount = recurringBooking.Amount;
            RecurringType = RecurringTypes[recurringBooking.RecurringType];
            EndDate = recurringBooking.EndDate;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Text { get; set; } = string.Empty;
        public string CategoryId { get; set; } = Guid.NewGuid().ToString();
        public bool IsCost { get; set; }
        public decimal Amount { get; set; }
        public string RecurringType { get; set; } = RecurringTypes[Gwiasda.FiMa.RecurringType.Once];
        public DateTime? EndDate { get; set; }

        public Booking ToBooking()
            => MapBase(new Booking());

        public RecurringBooking ToRecurringBooking()
        {
            var recurringBooking = new RecurringBooking
            {
                RecurringType = RecurringTypes.FirstOrDefault(x => x.Value == RecurringType).Key,
                EndDate = EndDate
            };
            return MapBase(recurringBooking);
        }
        private T MapBase<T>(T booking) where T : Booking
        {
            booking.Id = Guid.Parse(Id);
            booking.Timestamp = Timestamp;
            booking.Text = Text;
            booking.CategoryId = Guid.Parse(CategoryId);
            booking.IsCost = IsCost;
            booking.Amount = Amount;
            return booking;
        }
    }
}