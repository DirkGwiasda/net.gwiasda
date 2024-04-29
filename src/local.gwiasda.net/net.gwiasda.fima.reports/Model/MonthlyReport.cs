namespace Net.Gwiasda.FiMa
{
    public class MonthlyReport
    {
        public DateTime Month { get; set; } = DateTime.MinValue;
        public List<CategoryReport> CostCategoryReports { get; } = new List<CategoryReport>();
        public List<CategoryReport> IncomeCategoryReports { get; } = new List<CategoryReport>();
    }
}