using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa.Categories
{
    public class DeleteCategoryWorkflow : CategoryWorkflow, IDeleteCategoryWorkflow
    {
        private readonly ICategoryManager _categoryManager;

        public DeleteCategoryWorkflow(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager ?? throw new ArgumentNullException(nameof(categoryManager));
        }

        public async Task DeleteAsync(FinanceCategory category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            var isCostCategory = category is CostCategory;

            if (isCostCategory)
            {
                var costCategories = (await _categoryManager.GetCategoriesAsync<CostCategory>()).ToList();
                var categories = costCategories.Cast<FinanceCategory>().ToList();

                var item = categories.FirstOrDefault(cat => cat.Id == category.Id);
                if (item == null) return;

                categories.Remove(item);
                RecalculatePositions(categories, category.ParentId, category.Position);
                HandleChilds(categories, category);

                await _categoryManager.UpdateCategoriesAsync(categories.Cast<CostCategory>().ToList());
            }
            else
            {
                var incomeCategories = (await _categoryManager.GetCategoriesAsync<IncomeCategory>()).ToList();
                var categories = incomeCategories.Cast<FinanceCategory>().ToList();

                var item = categories.FirstOrDefault(cat => cat.Id == category.Id);
                if (item == null) return;

                categories.Remove(item);
                RecalculatePositions(categories, category.ParentId, category.Position);
                HandleChilds(categories, category);

                await _categoryManager.UpdateCategoriesAsync(categories.Cast<IncomeCategory>().ToList());
            }
        }
        private void HandleChilds(List<FinanceCategory> categories, FinanceCategory category)
        {
            var childs = categories.Where(cat => cat.ParentId == category.Id).ToList();
            var position = categories.Where(c => c.ParentId == category.ParentId).Count();
            foreach (var child in childs)
            {
                child.ParentId = category.ParentId;
                child.Position = position++;
                base.CalculateHierarchy(categories, child);
                RecalculateChildrenHistory(categories, category);
            }
        }
        private void RecalculateChildrenHistory(List<FinanceCategory> categories, FinanceCategory category)
        {
            var childs = categories.Where(cat => cat.ParentId == category.Id).ToList();
            foreach (var child in childs)
            {
                base.CalculateHierarchy(categories, child);
                RecalculateChildrenHistory(categories, child);
            }
        }
        private void RecalculatePositions(List<FinanceCategory> categories, Guid? parentId, int deletedPosition)
        {
            foreach(var category in categories.Where(cat => cat.ParentId == parentId && cat.Position > deletedPosition))
            {
                category.Position--;
            }
        }
    }
}