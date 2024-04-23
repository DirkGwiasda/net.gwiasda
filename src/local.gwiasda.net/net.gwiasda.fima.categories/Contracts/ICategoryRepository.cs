using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa
{
    public interface ICategoryRepository
    {
        Task<CostCategory> CreateCostCategoryAsync(CostCategory costCategory);
        Task<IncomeCategory> CreateIncomeCategoryAsync(IncomeCategory incomeCategory);
        Task DeleteCostCategoryAsync(Guid id);
        Task DeleteIncomeCategoryAsync(Guid id);
        Task<IEnumerable<CostCategory>> GetCostCategoriesAsync();
        Task<IEnumerable<IncomeCategory>> GetIncomeCategoriesAsync();
        Task<CostCategory> UpdateCostCategoryAsync(CostCategory costCategory);
        Task<IncomeCategory> UpdateIncomeCategoryAsync(IncomeCategory incomeCategory);
    }
}