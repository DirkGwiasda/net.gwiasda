namespace Net.Gwiasda.FiMa
{
    public class CategoryReport
    {
        public FinanceCategory Category { get; set; }
        public bool IsCost { get; set; }
        public decimal Sum { get; set; }
        public List<CategoryReport> ChildCategories { get; } = new List<CategoryReport>();
        public List<Booking> Bookings { get; } = new List<Booking>();
    }
}