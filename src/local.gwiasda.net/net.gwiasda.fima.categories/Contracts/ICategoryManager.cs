namespace Net.Gwiasda.FiMa
{
    public interface ICategoryManager
    {
        Task<T> CreateCategoryAsync<T>(T category) where T : FinanceCategory;
        Task DeleteCategoryAsync<T>(Guid id) where T : FinanceCategory;
        Task<IEnumerable<T>> GetCategoriesAsync<T>() where T : FinanceCategory;
        Task<T> UpdateCategoryAsync<T>(T category) where T : FinanceCategory;
        
    }
}