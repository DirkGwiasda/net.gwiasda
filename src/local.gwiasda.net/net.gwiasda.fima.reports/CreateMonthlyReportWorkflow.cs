using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa
{
    public class CreateMonthlyReportWorkflow : ICreateMonthlyReportWorkflow
    {
        private readonly ICategoryManager _categoryManager;
        private readonly IGetBookingsFromMonthWorkflow _getBookingsFromMonthWorkflow;

        public CreateMonthlyReportWorkflow(ICategoryManager categoryManager, IGetBookingsFromMonthWorkflow getBookingsFromMonthWorkflow)
        {
            _categoryManager = categoryManager ?? throw new ArgumentNullException(nameof(categoryManager));
            _getBookingsFromMonthWorkflow = getBookingsFromMonthWorkflow ?? throw new ArgumentNullException(nameof(getBookingsFromMonthWorkflow));
        }

        public async Task<MonthlyReport> CreateMonthlyReportAsync(DateTime month)
        {
            var result = new MonthlyReport { Month = month };

            var categories = await GetCategoriesAsync();
            var bookings = await _getBookingsFromMonthWorkflow.GetBookingsFromMonthAsync(month);
            var costCategoryReports = GetCategoryReports(true, bookings, categories);
            var incomeCategoryReports = GetCategoryReports(false, bookings, categories);

            result.CostCategoryReports.AddRange(CreateCategoryTree(costCategoryReports));
            result.IncomeCategoryReports.AddRange(CreateCategoryTree(incomeCategoryReports));

            ForceParentCategoriesExist(result.CostCategoryReports, categories, true);
            ForceParentCategoriesExist(result.IncomeCategoryReports, categories, false);

            CreateCategoryTree(result.CostCategoryReports);
            CreateCategoryTree(result.IncomeCategoryReports);

            return result;
        }

        internal List<CategoryReport> CreateCategoryTree(List<CategoryReport> categoryReports)
        {
            var result = new List<CategoryReport>();
            var handledIds = new List<Guid>();
            foreach (var categoryReport in categoryReports.Where(cr => cr.Category.ParentId == null))
            {
                result.Add(categoryReport);
                handledIds.Add(categoryReport.Category.Id);
                HandleChildCategories(categoryReport, categoryReports, handledIds);
            }

            result.Sort((a, b) => a.Category.Name.CompareTo(b.Category.Name));
            return result;
        }
        internal void HandleChildCategories(CategoryReport parentCategoryReport, List<CategoryReport> categoryReports, List<Guid> handledIds)
        {
            foreach (var categoryReport in categoryReports.Where(cr => cr.Category.ParentId == parentCategoryReport.Category.Id))
            {
                if(handledIds.Contains(categoryReport.Category.Id))
                    continue;

                parentCategoryReport.ChildCategories.Add(categoryReport);
                handledIds.Add(categoryReport.Category.Id);
                parentCategoryReport.Sum += categoryReport.Sum;

                HandleChildCategories(categoryReport, categoryReports, handledIds);
            }

            parentCategoryReport.ChildCategories.Sort((a, b) => a.Category.Position.CompareTo(b.Category.Position));
        }
        internal void ForceParentCategoriesExist(List<CategoryReport> categoryReports, IEnumerable<FinanceCategory> categories, bool isCost)
        {
            var handledIds = categoryReports.Select(cr => cr.Category.Id).ToList();
            foreach(var categoryReport in categoryReports)
            {
                ForceParentCategoryIsHandled(categoryReport.Category.ParentId, categoryReports, handledIds, categories, isCost);
            }
            
        }
        internal void ForceParentCategoryIsHandled(Guid? parentCategoryId, List<CategoryReport> categoryReports, List<Guid> handledIds, IEnumerable<FinanceCategory> categories, bool isCost)
        {
            if(parentCategoryId == null)
                return;

            if (!handledIds.Contains(parentCategoryId.Value))
            {
                var category = categories.FirstOrDefault(c => c.Id == parentCategoryId.Value);
                if (category == null)
                    throw new InvalidOperationException($"No category found with id '{parentCategoryId.Value}' - isCost: {isCost}.");
                var categoryReport = new CategoryReport { Category = category, IsCost = isCost };
                categoryReports.Add(categoryReport);
                handledIds.Add(categoryReport.Category.Id);
                ForceParentCategoryIsHandled(category.ParentId, categoryReports, handledIds, categories, isCost);
            }
        }

        internal List<CategoryReport> GetCategoryReports(bool isCostCategory, IEnumerable<Booking> bookings, IEnumerable<FinanceCategory> categories)
        { 
            var result = new List<CategoryReport>();

            foreach(var booking in bookings.Where(b => b.IsCost == isCostCategory))
            {
                var categoryReport = GetCategoryReport(booking, result, categories);
                categoryReport.Bookings.Add(booking);
                categoryReport.Sum += booking.Amount;
            }

            return result;
        }
        internal CategoryReport GetCategoryReport(Booking booking, List<CategoryReport> categoryReports, IEnumerable<FinanceCategory> categories)
        {
            var categoryReport = categoryReports.FirstOrDefault(cr => cr.Category.Id == booking.CategoryId);
            if(categoryReport == null)
            {
                var category = categories.FirstOrDefault(c => c.Id == booking.CategoryId);
                if (category == null)
                    throw new InvalidOperationException($"Category '{booking.CategoryId}' for booking '{booking.Id}' from '{booking.Timestamp}' not found!");
                else
                {
                    categoryReport = new CategoryReport { Category = category, IsCost = booking.IsCost };
                    categoryReports.Add(categoryReport);
                }
            }
            return categoryReport;
        }
        internal async Task<IEnumerable<FinanceCategory>> GetCategoriesAsync()
        {
            var costCategories = (await _categoryManager.GetCategoriesAsync<CostCategory>());
            var incomeCategories = (await _categoryManager.GetCategoriesAsync<IncomeCategory>());

            var categories = new List<FinanceCategory>();
            categories.AddRange(costCategories);
            categories.AddRange(incomeCategories);

            return categories;
        }
    }
}