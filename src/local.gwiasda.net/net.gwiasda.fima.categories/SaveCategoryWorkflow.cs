using Net.Gwiasda.FiMa.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa
{
    public class SaveCategoryWorkflow : CategoryWorkflow, ISaveCategoryWorkflow
    {
        private readonly ICategoryValidator _categoryValidator;
        private readonly ICategoryManager _categoryManager;

        public SaveCategoryWorkflow(ICategoryValidator categoryValidator, ICategoryManager categoryManager)
        {
            _categoryValidator = categoryValidator ?? throw new ArgumentNullException(nameof(categoryValidator));
            _categoryManager = categoryManager ?? throw new ArgumentNullException(nameof(categoryManager));
        }

        public async Task<FinanceCategory> SaveAsync(FinanceCategory category)
        {
            if(category == null) throw new ArgumentNullException(nameof(category));

            _categoryValidator.ValidateCategory(category);

            var isCostCategory = category is CostCategory;

            if (isCostCategory)
            {
                var costCategories = (await _categoryManager.GetCategoriesAsync<CostCategory>()).ToList();
                var categories = costCategories.Cast<FinanceCategory>().ToList();

                base.CalculateHierarchy(categories, category);
                PrepareAndInsertCategory(categories, category);

                await _categoryManager.UpdateCategoriesAsync(categories.Cast<CostCategory>().ToList());
            }
            else
            {
                var incomeCategories = (await _categoryManager.GetCategoriesAsync<IncomeCategory>()).ToList();
                var categories = incomeCategories.Cast<FinanceCategory>().ToList();

                base.CalculateHierarchy(categories, category);
                PrepareAndInsertCategory(categories, category);

                await _categoryManager.UpdateCategoriesAsync(categories.Cast<IncomeCategory>().ToList());
            }

            return category;
        }
        
        private void PrepareAndInsertCategory(List<FinanceCategory> categories, FinanceCategory category)
        {
            var existingCategory = categories.FirstOrDefault(cat => cat.Id == category.Id);
            if (existingCategory != null)
                base.RemoveCategoryAndResort(categories, existingCategory);

            InsertCategory(categories, category);
        }
        private void InsertCategory(List<FinanceCategory> categories, FinanceCategory category)
        {
            var childs = categories.Where(cat => cat.ParentId == category.ParentId);
            if (category.Position < 0 || category.Position > childs.Count())
                category.Position = childs.Count();

            foreach(var cat in childs.Where(cat => cat.Position >= category.Position))
                cat.Position++;

            categories.Add(category);
        }
    }
}