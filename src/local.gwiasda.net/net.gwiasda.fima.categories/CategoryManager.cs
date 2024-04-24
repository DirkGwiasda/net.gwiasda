using System.ComponentModel.DataAnnotations;

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

        public async Task<T> CreateCategoryAsync<T>(T category) where T : FinanceCategory
        {
            _categoryValidator.ValidateCategory(category);

            return await _categoryRepository.CreateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync<T>(Guid id) where T : FinanceCategory
         => await _categoryRepository.DeleteCategoryAsync<T>(id);

        public async Task<IEnumerable<T>> GetCategoriesAsync<T>() where T : FinanceCategory
        { 
            var categories = await _categoryRepository.GetCategoriesAsync<T>();
            return SortCategories(categories.ToList());
        }

        public async Task<T> UpdateCategoryAsync<T>(T category) where T : FinanceCategory
        {
            _categoryValidator.ValidateCategory(category);

            return await _categoryRepository.UpdateCategoryAsync(category);
        }
        public async Task UpdateCategoriesAsync<T>(IEnumerable<T> categories) where T : FinanceCategory
        {
            await _categoryRepository.UpdateCategoriesAsync(categories);
        }

        private List<T> SortCategories<T>(List<T> categories) where T : FinanceCategory
        {
            if(categories == null) throw new ArgumentNullException(nameof(categories));

            var result = new List<T>();

            SortByParent(categories, result, null);

            return result;
        }
        private void SortByParent<T>(List<T> categories, List<T> sorted, Guid? parentId) where T : FinanceCategory
        {
            if(categories == null) throw new ArgumentNullException(nameof(categories));
            if(sorted == null) throw new ArgumentNullException(nameof(sorted));

            var childs = categories.Where(c => c.ParentId == parentId).ToList();
            childs.Sort((c1, c2) => c1.Position.CompareTo(c2.Position));
            foreach(var child in childs)
            {
                sorted.Add(child);
                SortByParent(categories, sorted, child.Id);
                categories.Remove(child);
            }
        }
    }
}