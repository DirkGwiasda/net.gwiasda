using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa.Categories
{
    public class CategoryValidator : ICategoryValidator
    {
        public void ValidateCategory(FinanceCategory category)
        {
            if(category == null) throw new ArgumentNullException(nameof(category));
            if(string.IsNullOrWhiteSpace(category.Name)) throw new ArgumentException("Category name must not be empty", nameof(category.Name));
            if(string.IsNullOrWhiteSpace(category.Description)) throw new ArgumentException("Category description must not be empty", nameof(category.Description));
            if(category.Position < 0) throw new ArgumentException("Category position must be greater or equal to 0", nameof(category.Position));
        }
    }
}