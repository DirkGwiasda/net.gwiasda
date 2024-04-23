using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryValidator _categoryValidator;

        public CategoryManager(ICategoryRepository categoryRepository, ICategoryValidator categoryValidator)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _categoryValidator = categoryValidator ?? throw new ArgumentNullException(nameof(categoryValidator));
        }

        public async Task<CostCategory> CreateCostCategoryAsync(CostCategory costCategory)
        {
            if(costCategory == null) throw new ArgumentNullException(nameof(costCategory));
            if(string.IsNullOrWhiteSpace(costCategory.Name)) throw new ArgumentException("Cost category name must not be empty", nameof(costCategory.Name));    
            if(string.IsNullOrWhiteSpace(costCategory.Description)) throw new ArgumentException("Cost category description must not be empty", nameof(costCategory.Description));

            return await _categoryRepository.CreateCostCategoryAsync(costCategory);
        }

        public Task<IncomeCategory> CreateIncomeCategoryAsync(IncomeCategory incomeCategory)
        {
            if(incomeCategory == null) throw new ArgumentNullException(nameof(incomeCategory));
            if(string.IsNullOrWhiteSpace(incomeCategory.Name)) throw new ArgumentException("Income category name must not be empty", nameof(incomeCategory.Name));
            if(string.IsNullOrWhiteSpace(incomeCategory.Description)) throw new ArgumentException("Income category description must not be empty", nameof(incomeCategory.Description));

            return _categoryRepository.CreateIncomeCategoryAsync(incomeCategory);
        }

        public async Task DeleteCostCategoryAsync(Guid id)
         => await _categoryRepository.DeleteCostCategoryAsync(id);

        public async Task DeleteIncomeCategoryAsync(Guid id)
         => await _categoryRepository.DeleteIncomeCategoryAsync(id);

        public async Task<IEnumerable<CostCategory>> GetCostCategoriesAsync()
         => await _categoryRepository.GetCostCategoriesAsync();

        public async Task<IEnumerable<IncomeCategory>> GetIncomeCategoriesAsync()
         => await _categoryRepository.GetIncomeCategoriesAsync();

        public async Task<CostCategory> UpdateCostCategoryAsync(CostCategory costCategory)
        {
            if(costCategory == null) throw new ArgumentNullException(nameof(costCategory));
            if(string.IsNullOrWhiteSpace(costCategory.Name)) throw new ArgumentException("Cost category name must not be empty", nameof(costCategory.Name));
            if(string.IsNullOrWhiteSpace(costCategory.Description)) throw new ArgumentException("Cost category description must not be empty", nameof(costCategory.Description));

            return await _categoryRepository.UpdateCostCategoryAsync(costCategory);
        }

        public async Task<IncomeCategory> UpdateIncomeCategoryAsync(IncomeCategory incomeCategory)
        {
            if(incomeCategory == null) throw new ArgumentNullException(nameof(incomeCategory));
            if(string.IsNullOrWhiteSpace(incomeCategory.Name)) throw new ArgumentException("Income category name must not be empty", nameof(incomeCategory.Name));
            if(string.IsNullOrWhiteSpace(incomeCategory.Description)) throw new ArgumentException("Income category description must not be empty", nameof(incomeCategory.Description));

            return await _categoryRepository.UpdateIncomeCategoryAsync(incomeCategory);
        }
    }
}