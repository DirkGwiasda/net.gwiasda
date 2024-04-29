using Net.Gwiasda.FiMa;

namespace Net.Gwiasda.Local.UI.ViewModel.FiMa
{
    public class CategoryReportViewModel
    {
        public CategoryReportViewModel() { }
        public CategoryReportViewModel(CategoryReport categoryReport)
        {
            Category = new FinanceCategoryViewModel(categoryReport.Category);
            IsCost = categoryReport.IsCost;
            Sum = categoryReport.Sum;
            foreach (var childCategory in categoryReport.ChildCategories)
            {
                ChildCategories.Add(new CategoryReportViewModel(childCategory));
            }
            foreach (var booking in categoryReport.Bookings)
            {
                Bookings.Add(booking);
            }
        }
        public FinanceCategoryViewModel Category { get; set; }
        public bool IsCost { get; set; }
        public decimal Sum { get; set; }
        public List<CategoryReportViewModel> ChildCategories { get; } = new List<CategoryReportViewModel>();
        public List<Booking> Bookings { get; } = new List<Booking>();
    }
}