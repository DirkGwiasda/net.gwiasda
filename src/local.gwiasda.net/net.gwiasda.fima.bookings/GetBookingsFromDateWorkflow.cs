using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa
{
    public class GetBookingsFromDateWorkflow : IGetBookingsFromDateWorkflow
    {
        private readonly IBookingManager _bookingManager;
        private readonly ICategoryManager _categoryManager;

        public GetBookingsFromDateWorkflow(IBookingManager bookingManager, ICategoryManager categoryManager)
        {
            _bookingManager = bookingManager ?? throw new ArgumentNullException(nameof(bookingManager));
            _categoryManager = categoryManager ?? throw new ArgumentNullException(nameof(categoryManager));
        }

        public async Task<Dictionary<string, List<Booking>>> GetBookingsFromDay(DateTime day)
        {
            var categories = await GetAllCategories();

            var bookingsFromDay = await _bookingManager.GetBookingsFromDay(day);

            var result = new Dictionary<string, List<Booking>>();
            foreach(var booking in bookingsFromDay)
            {
                var category = categories.FirstOrDefault(c => c.Id == booking.CategoryId);

                var keyName = category?.Name ?? booking.CategoryId.ToString(); 

                if (!result.ContainsKey(keyName))
                    result.Add(keyName, new List<Booking>());

                result[keyName].Add(booking);
            }
            return result;
        }

        private async Task<List<FinanceCategory>> GetAllCategories()
        {
            var costCategories = await _categoryManager.GetCategoriesAsync<CostCategory>();
            var incomeCategories = await _categoryManager.GetCategoriesAsync<IncomeCategory>();

            var financialCategories = new List<FinanceCategory>();
            financialCategories.AddRange(costCategories);
            financialCategories.AddRange(incomeCategories);

            return financialCategories;
        }
    }
}