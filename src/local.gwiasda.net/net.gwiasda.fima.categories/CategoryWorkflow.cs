using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa.Categories
{
    public abstract class CategoryWorkflow
    {
        protected void RemoveCategoryAndResort(List<FinanceCategory> categories, FinanceCategory category)
        {
            categories.Remove(category);
            var cats = categories.Where(cat => cat.ParentId == category.ParentId).ToList();
            for (var i = 0; i < cats.Count; i++)
                cats[i].Position = i;
        }
        protected void CalculateHierarchy(List<FinanceCategory> categories, FinanceCategory category)
        {
            var hierarchy = 0;
            var currentCategory = category;
            while (currentCategory?.ParentId != null)
            {
                hierarchy++;
                currentCategory = categories.FirstOrDefault(cat => cat.Id == currentCategory.ParentId);
            }
            category.Hierarchy = hierarchy;
        }
    }
}