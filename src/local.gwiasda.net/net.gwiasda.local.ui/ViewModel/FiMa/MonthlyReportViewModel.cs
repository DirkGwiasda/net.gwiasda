using Net.Gwiasda.FiMa;

namespace Net.Gwiasda.Local.UI.ViewModel.FiMa
{
    public class MonthlyReportViewModel
    {
        public MonthlyReportViewModel() { }
        public MonthlyReportViewModel(MonthlyReport source)
        {
            Month = source.Month;
            foreach (var costCategoryReport in source.CostCategoryReports)
            {
                CostCategoryReports.Add(new CategoryReportViewModel(costCategoryReport));
            }
            foreach (var incomeCategoryReport in source.IncomeCategoryReports)
            {
                IncomeCategoryReports.Add(new CategoryReportViewModel(incomeCategoryReport));
            }
        }

        public DateTime Month { get; set; } = DateTime.MinValue;
        public List<CategoryReportViewModel> CostCategoryReports { get; } = new List<CategoryReportViewModel>();
        public List<CategoryReportViewModel> IncomeCategoryReports { get; } = new List<CategoryReportViewModel>();
    }
}