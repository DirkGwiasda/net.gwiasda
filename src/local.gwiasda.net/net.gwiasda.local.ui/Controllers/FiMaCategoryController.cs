using Microsoft.AspNetCore.Mvc;
using Net.Gwiasda.FiMa;
using Net.Gwiasda.Local.UI.ViewModel.FiMa;
using Net.Gwiasda.Logging;

namespace Net.Gwiasda.Local.UI.Controllers
{
    public class FiMaCategoryController : Controller
    {
        private const string APP_NAME = "FiMa";
        private readonly ILoggingManager _loggingManager;
        private readonly ICategoryManager _categoryManager;
        private readonly ISaveCategoryWorkflow _saveCategoryWorkflow;
        private readonly IDeleteCategoryWorkflow _deleteCategoryWorkflow;

        public FiMaCategoryController(ILoggingManager loggingManager, ICategoryManager categoryManager, 
            ISaveCategoryWorkflow saveCategoryWorkflow, IDeleteCategoryWorkflow deleteCategoryWorkflow)
        {
            _loggingManager = loggingManager ?? throw new ArgumentNullException(nameof(loggingManager));
            _categoryManager = categoryManager ?? throw new ArgumentNullException(nameof(categoryManager));
            _saveCategoryWorkflow = saveCategoryWorkflow ?? throw new ArgumentNullException(nameof(saveCategoryWorkflow));
            _deleteCategoryWorkflow = deleteCategoryWorkflow ?? throw new ArgumentNullException(nameof(deleteCategoryWorkflow));
        }

        public Task<string> Ping()
        {
            return Task.FromResult($"{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")} Pong from FiMaCategoryController");
        }

        public async Task<IEnumerable<FinanceCategoryViewModel>> GetCostCategories()
        {
            try
            {
                return (await _categoryManager.GetCategoriesAsync<CostCategory>())
                    .Select(cat => new FinanceCategoryViewModel(cat)).ToList();
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc);
                throw;
            }
        }
        public async Task<IEnumerable<FinanceCategoryViewModel>> GetIncomeCategories()
        {
            try
            {
                return (await _categoryManager.GetCategoriesAsync<IncomeCategory>())
                    .Select(cat => new FinanceCategoryViewModel(cat)).ToList();
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc);
                throw;
            }
        }

        [HttpPost]
        public async Task<FinanceCategoryViewModel> Save([FromBody] FinanceCategoryViewModel categoryViewModel)
        {
            try
            {
                if (categoryViewModel == null) throw new ArgumentNullException(nameof(categoryViewModel));
                var category = categoryViewModel.ToCategory();

                category = await _saveCategoryWorkflow.SaveAsync(category).ConfigureAwait(true);

                return new FinanceCategoryViewModel(category);
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc).ConfigureAwait(true);
                throw;
            }
        }
        [HttpPost]
        public async Task Delete([FromBody] FinanceCategoryViewModel categoryViewModel)
        {
            try
            {
                if(categoryViewModel == null) throw new ArgumentNullException(nameof(categoryViewModel));
                var category = categoryViewModel.ToCategory();

                await _deleteCategoryWorkflow.DeleteAsync(category);
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc).ConfigureAwait(true);
                throw;
            }
        }
    }
}