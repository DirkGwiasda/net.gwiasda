using Net.Gwiasda.FiMa;
using System.Text.Json;

namespace Net.Gwiasda.Local.Repository
{
    public class FiMaFileSystemCategoryRepository : ICategoryRepository
    {
        internal const string RootDataDirectory = "gwiasda_local_data";
        internal const string FiMaDirectory = "fima";
        internal const string CostCategoriesFileName = "cost_categories.fima";
        internal const string IncomeCategoriesFileName = "income_categories.fima";

        private readonly object _sync = new object();

        public Task<T> CreateCategoryAsync<T>(T category) where T : FinanceCategory
        {
            lock(_sync)
            {
                var categories = GetCategoriesAsync<T>().Result.ToList();
                categories.Add(category);
                WriteCategoriesToFile(categories);
            }
            return Task.FromResult(category);
        }

        public Task DeleteCategoryAsync<T>(Guid id) where T : FinanceCategory
        {
            lock(_sync)
            {
                var categories = GetCategoriesAsync<T>().Result.ToList();
                var costCategory = categories.FirstOrDefault(c => c.Id == id);
                if(costCategory != null)
                {
                    categories.Remove(costCategory);
                    WriteCategoriesToFile(categories);
                }
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetCategoriesAsync<T>() where T : FinanceCategory
        {
            var fileName = typeof(T) == typeof(CostCategory) ? GetCostCategoriesFileName() : GetIncomeCategoriesFileName();
            if(!File.Exists(fileName)) return new List<T>();
            var json = await File.ReadAllTextAsync(fileName);
            var categories = JsonSerializer.Deserialize<IEnumerable<T>>(json);
            return categories ?? new List<T>();
        }

        public Task<T> UpdateCategoryAsync<T>(T category) where T : FinanceCategory
        {
            lock (_sync)
            {
                var categories = GetCategoriesAsync<T>().Result.ToList();
                var existingCategory = categories.FirstOrDefault(c => c.Id == category.Id);
                if (existingCategory != null)
                {
                    categories.Remove(existingCategory);
                    categories.Add(category);
                    WriteCategoriesToFile(categories);
                }
            }
            return Task.FromResult(category);
        }

        private void WriteCategoriesToFile<T>(IEnumerable<T> categories)
        {
            var json = JsonSerializer.Serialize(categories);
            var fileName = typeof(T) == typeof(CostCategory) ? GetCostCategoriesFileName() : GetIncomeCategoriesFileName();
            File.WriteAllText(fileName, json);
        }

        private string GetBaseDirectory()
        {
            var dir = Path.Combine(RootDataDirectory, FiMaDirectory);
            if(!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir;
        }
        private string GetCostCategoriesFileName() => Path.Combine(GetBaseDirectory(), CostCategoriesFileName);
        private string GetIncomeCategoriesFileName() => Path.Combine(GetBaseDirectory(), IncomeCategoriesFileName);
    }
}