using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa.Categories
{
    public class CategoryValidator : ICategoryValidator
    {
        public void ValidateCostCategory(CostCategory costCategory)
        {
            ValidateCategory(costCategory);
        }
        public void ValidateIncomeCategory(IncomeCategory incomeCategory)
        {
            ValidateCategory(incomeCategory);
        }

        private void ValidateCategory(FinanceCategory category)
        {
            if(category == null) throw new ArgumentNullException(nameof(category));
            if(string.IsNullOrWhiteSpace(category.Name)) throw new ArgumentException("Category name must not be empty", nameof(category.Name));
            if(string.IsNullOrWhiteSpace(category.Description)) throw new ArgumentException("Category description must not be empty", nameof(category.Description));
        }
    }
}