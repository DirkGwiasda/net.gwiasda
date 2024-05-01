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
        private readonly IBookingManager _bookingManager;
        private readonly IGetBookingsFromMonthWorkflow _getBookingsFromMonthWorkflow;

        public CreateMonthlyReportWorkflow(ICategoryManager categoryManager, 
            IGetBookingsFromMonthWorkflow getBookingsFromMonthWorkflow, IBookingManager bookingManager)
        {
            _categoryManager = categoryManager ?? throw new ArgumentNullException(nameof(categoryManager));
            _getBookingsFromMonthWorkflow = getBookingsFromMonthWorkflow ?? throw new ArgumentNullException(nameof(getBookingsFromMonthWorkflow));
            _bookingManager = bookingManager ?? throw new ArgumentNullException(nameof(bookingManager));
        }

        public async Task<MonthlyReport> CreateMonthlyReportAsync(DateTime month)
        {
            var result = new MonthlyReport { Month = month };

            var categories = await GetCategoriesAsync();
            var bookings = await _getBookingsFromMonthWorkflow.GetBookingsFromMonthAsync(month);
            var costCategoryReports = GetCategoryReports(true, bookings, categories);
            var incomeCategoryReports = GetCategoryReports(false, bookings, categories);

            await AddRecurringBookings(month, costCategoryReports, categories, true);
            await AddRecurringBookings(month,incomeCategoryReports, categories, false);

            ForceParentCategoriesExist(costCategoryReports, categories);
            ForceParentCategoriesExist(incomeCategoryReports, categories);

            AddSumToParentSum(costCategoryReports);
            AddSumToParentSum(incomeCategoryReports);

            var trees = CreateCategoryTrees(costCategoryReports);
            result.CostCategoryReports.AddRange(trees);

            trees = CreateCategoryTrees(incomeCategoryReports);
            result.IncomeCategoryReports.AddRange(trees);

            return result;
        }

        internal List<CategoryReport> CreateCategoryTrees(List<CategoryReport> categoryReports)
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

                HandleChildCategories(categoryReport, categoryReports, handledIds);
            }

            parentCategoryReport.ChildCategories.Sort((a, b) => a.Category.Position.CompareTo(b.Category.Position));
        }
        internal void ForceParentCategoriesExist(List<CategoryReport> categoryReports, IEnumerable<FinanceCategory> categories)
        {
            var handledIds = categoryReports.Select(cr => cr.Category.Id).ToList();
            var items2Add = new List<CategoryReport>();
            foreach(var categoryReport in categoryReports)
            {
                ForceParentCategoryExist(categoryReport, categoryReports, handledIds, categories, items2Add);
            }
            categoryReports.AddRange(items2Add);
        }
        internal void ForceParentCategoryExist(CategoryReport categoryReport, List<CategoryReport> categoryReports, List<Guid> handledIds, IEnumerable<FinanceCategory> categories, List<CategoryReport> items2add)
        {
            if(categoryReport?.Category?.ParentId == null)
                return;

            if (!handledIds.Contains(categoryReport.Category.ParentId.Value))
            {
                var parentCategory = categories.FirstOrDefault(c => c.Id == categoryReport.Category.ParentId.Value);
                if (parentCategory == null)
                    throw new InvalidOperationException($"No category found with id '{categoryReport.Category.ParentId.Value}' - isCost: {categoryReport.IsCost}.");
                var parentCategoryReport = new CategoryReport { Category = parentCategory, IsCost = categoryReport.IsCost };
                items2add.Add(parentCategoryReport);
                handledIds.Add(parentCategoryReport.Category.Id);
                ForceParentCategoryExist(parentCategoryReport, categoryReports, handledIds, categories, items2add);
            }
        }
        internal void AddSumToParentSum(List<CategoryReport> categoryReports)
        {
            if(categoryReports == null || categoryReports.Count == 0)
                return;

            var maxHierarchy = categoryReports.Max(cr => cr.Category.Hierarchy);
            for (int i = maxHierarchy; i > 0; i--)
            {
                var hierarchyItems = categoryReports.Where(cr => cr.Category.Hierarchy == i);
               foreach(var currentCategoryReport in hierarchyItems)
                {
                    var parent = categoryReports.FirstOrDefault(cr => cr.Category.Id == currentCategoryReport.Category.ParentId);
                    if(parent != null)
                    {
                        parent.Sum += currentCategoryReport.Sum;
                    }
                }
            }
        }
        internal async Task AddRecurringBookings(DateTime month, List<CategoryReport> categoryReports, IEnumerable<FinanceCategory> categories, bool handleCosts)
        {
            var recurringBookings = await _bookingManager.GetRecurringBookings();
            foreach (var recurringBooking in recurringBookings)
            {
                if(handleCosts != recurringBooking.IsCost)
                    continue;

                if(!IsInMonth(recurringBooking, month))
                    continue;

                var categoryReport = GetCategoryReport(recurringBooking, categoryReports, categories);
                categoryReport.RecurringBookings.Add(recurringBooking);
                categoryReport.Sum += recurringBooking.Amount;
            }
        }

        internal bool IsInMonth(RecurringBooking recurringBooking, DateTime month)
        {
            var bookingMonth = new DateTime(recurringBooking.Timestamp.Year, recurringBooking.Timestamp.Month, 1);
            month = new DateTime(month.Year, month.Month, 1);

            if (bookingMonth > month) 
                return false;

            if (bookingMonth == month) 
                return true;

            if (recurringBooking.EndDate.HasValue)
            {
                var endDate = new DateTime(recurringBooking.EndDate.Value.Year, recurringBooking.EndDate.Value.Month, 1);
                if (endDate < month)
                    return false;
            }

            while(bookingMonth < month)
            {
                if (recurringBooking.RecurringType == RecurringType.Yearly)
                    bookingMonth = bookingMonth.AddYears(1);
                else if (recurringBooking.RecurringType == RecurringType.Quarter)
                    bookingMonth = bookingMonth.AddMonths(3);
                else if (recurringBooking.RecurringType == RecurringType.Monthly)
                    bookingMonth = bookingMonth.AddMonths(1);
                else
                    return false;
                
                if (bookingMonth == month)
                    return true;
            }
            return false;
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