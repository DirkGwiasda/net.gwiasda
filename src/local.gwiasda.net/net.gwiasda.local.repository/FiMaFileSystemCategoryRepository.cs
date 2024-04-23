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

        public Task<CostCategory> CreateCostCategoryAsync(CostCategory costCategory)
        {
            lock(_sync)
            {
                var costCategories = GetCostCategoriesAsync().Result.ToList();
                costCategories.Add(costCategory);
                WriteCostCategoriesToFile(costCategories);
            }
            return Task.FromResult(costCategory);
        }

        public Task<IncomeCategory> CreateIncomeCategoryAsync(IncomeCategory incomeCategory)
        {
            lock (_sync)
            {
                var incomeCategories = GetIncomeCategoriesAsync().Result.ToList();
                incomeCategories.Add(incomeCategory);
                WriteIncomeCategoriesToFile(incomeCategories);
            }
            return Task.FromResult(incomeCategory);
        }

        public Task DeleteCostCategoryAsync(Guid id)
        {
            lock(_sync)
            {
                var costCategories = GetCostCategoriesAsync().Result.ToList();
                var costCategory = costCategories.FirstOrDefault(c => c.Id == id);
                if(costCategory != null)
                {
                    costCategories.Remove(costCategory);
                    WriteCostCategoriesToFile(costCategories);
                }
            }
            return Task.CompletedTask;
        }

        public Task DeleteIncomeCategoryAsync(Guid id)
        {
            lock(_sync)
            {                 
                var incomeCategories = GetIncomeCategoriesAsync().Result.ToList();
                var incomeCategory = incomeCategories.FirstOrDefault(c => c.Id == id);
                if(incomeCategory != null)
                {
                    incomeCategories.Remove(incomeCategory);
                    WriteIncomeCategoriesToFile(incomeCategories);
                }
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<CostCategory>> GetCostCategoriesAsync()
        {
            var json = await File.ReadAllTextAsync(GetCostCategoriesFileName());
            var costCategories = JsonSerializer.Deserialize<IEnumerable<CostCategory>>(json);
            return costCategories ?? new List<CostCategory>();
        }

        public async Task<IEnumerable<IncomeCategory>> GetIncomeCategoriesAsync()
        {
            var json = await File.ReadAllTextAsync(GetIncomeCategoriesFileName());
            var incomeCategories = JsonSerializer.Deserialize<IEnumerable<IncomeCategory>>(json);
            return incomeCategories ?? new List<IncomeCategory>();
        }

        public Task<CostCategory> UpdateCostCategoryAsync(CostCategory costCategory)
        {
            lock (_sync)
            {
                var costCategories = GetCostCategoriesAsync().Result.ToList();
                var existingCostCategory = costCategories.FirstOrDefault(c => c.Id == costCategory.Id);
                if (existingCostCategory != null)
                {
                    costCategories.Remove(existingCostCategory);
                    costCategories.Add(costCategory);
                    WriteCostCategoriesToFile(costCategories);
                }
            }
            return Task.FromResult(costCategory);
        }

        public Task<IncomeCategory> UpdateIncomeCategoryAsync(IncomeCategory incomeCategory)
        {
            lock (_sync)
            {
                var incomeCategories = GetIncomeCategoriesAsync().Result.ToList();
                var existingIncomeCategory = incomeCategories.FirstOrDefault(c => c.Id == incomeCategory.Id);
                if (existingIncomeCategory != null)
                {
                    incomeCategories.Remove(existingIncomeCategory);
                    incomeCategories.Add(incomeCategory);
                    WriteIncomeCategoriesToFile(incomeCategories);
                }
            }
            return Task.FromResult(incomeCategory);
        }

        private void WriteCostCategoriesToFile(IEnumerable<CostCategory> costCategories)
        {
            var json = JsonSerializer.Serialize(costCategories);
            File.WriteAllText(GetCostCategoriesFileName(), json);
        }
        private void WriteIncomeCategoriesToFile(IEnumerable<IncomeCategory> incomeCategories)
        {
            var json = JsonSerializer.Serialize(incomeCategories);
            File.WriteAllText(GetIncomeCategoriesFileName(), json);
        }

        private string GetBaseDirectory() => Path.Combine(RootDataDirectory, FiMaDirectory);
        private string GetCostCategoriesFileName() => Path.Combine(GetBaseDirectory(), CostCategoriesFileName);
        private string GetIncomeCategoriesFileName() => Path.Combine(GetBaseDirectory(), IncomeCategoriesFileName);
    }
}