using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa
{
    public class CategorySaveWorkflow : ICategorySaveWorkflow
    {
        private readonly ICategoryValidator _categoryValidator;
        private readonly ICategoryManager _categoryManager;

        public CategorySaveWorkflow(ICategoryValidator categoryValidator, ICategoryManager categoryManager)
        {
            _categoryValidator = categoryValidator ?? throw new ArgumentNullException(nameof(categoryValidator));
            _categoryManager = categoryManager ?? throw new ArgumentNullException(nameof(categoryManager));
        }

        public async Task SaveAsync(FinanceCategory category)
        {
            if(category == null) throw new ArgumentNullException(nameof(category));

            _categoryValidator.ValidateCategory(category);

            var isCostCategory = category is CostCategory;

            if (isCostCategory)
            {
                var costCategories = (await _categoryManager.GetCategoriesAsync<CostCategory>()).ToList();
                var categories = costCategories.Cast<FinanceCategory>().ToList();

                CalculateHierarchy(categories, category);
                PrepareAndInsertCategory(categories, category);

                await _categoryManager.UpdateCategoriesAsync(categories.Cast<CostCategory>().ToList());
            }
            else
            {
                var incomeCategories = (await _categoryManager.GetCategoriesAsync<IncomeCategory>()).ToList();
                var categories = incomeCategories.Cast<FinanceCategory>().ToList();

                CalculateHierarchy(categories, category);
                PrepareAndInsertCategory(categories, category);

                await _categoryManager.UpdateCategoriesAsync(categories.Cast<IncomeCategory>().ToList());
            }
        }

        private void CalculateHierarchy(List<FinanceCategory> categories, FinanceCategory category)
        {
            var hierarchy = 0;
            var currentCategory = category;
            while(currentCategory?.ParentId != null)
            {
                hierarchy++;
                currentCategory = categories.FirstOrDefault(cat => cat.Id == currentCategory.ParentId);
            }
            category.Hierarchy = hierarchy;
        }

        private void PrepareAndInsertCategory(List<FinanceCategory> categories, FinanceCategory category)
        {
            var existingCategory = categories.FirstOrDefault(cat => cat.Id == category.Id);
            if (existingCategory != null)
                RemoveCategoryAndResort(categories, existingCategory);

            InsertCategory(categories, category);
        }

        private void InsertCategory(List<FinanceCategory> categories, FinanceCategory category)
        {
            if(category.Position < 0 || category.Position > categories.Count)
                category.Position = categories.Count;

            foreach(var cat in categories.Where(cat => cat.ParentId == category.ParentId && cat.Position >= category.Position))
                cat.Position++;

            categories.Add(category);
        }

        private void RemoveCategoryAndResort(List<FinanceCategory> categories, FinanceCategory category)
        {
            categories.Remove(category);
            foreach(var cat in categories.Where(cat => cat.ParentId == category.ParentId && cat.Position > category.Position))
                cat.Position--;
        }
    }
}