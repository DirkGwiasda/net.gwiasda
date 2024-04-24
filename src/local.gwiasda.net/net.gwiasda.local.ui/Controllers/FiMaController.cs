﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Net.Gwiasda.FiMa;
using Net.Gwiasda.Local.UI.ViewModel.FiMa;
using Net.Gwiasda.Local.UI.ViewModel.Logging;
using Net.Gwiasda.Logging;
using System.Collections.ObjectModel;

namespace Net.Gwiasda.Local.UI.Controllers
{
    public class FiMaController : Controller
    {
        private const string APP_NAME = "FiMa";
        private readonly ILoggingManager _loggingManager;
        private readonly ICategoryManager _categoryManager;
        private readonly ICategorySaveWorkflow _categorySaveWorkflow;

        public FiMaController(ILoggingManager loggingManager, ICategoryManager categoryManager, ICategorySaveWorkflow categorySaveWorkflow)
        {
            _loggingManager = loggingManager ?? throw new ArgumentNullException(nameof(loggingManager));
            _categoryManager = categoryManager ?? throw new ArgumentNullException(nameof(categoryManager));
            _categorySaveWorkflow = categorySaveWorkflow ?? throw new ArgumentNullException(nameof(categorySaveWorkflow));

        }

        public Task<string> Ping()
        {
            return Task.FromResult($"{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")} Pong from FiMaController");
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
                await _loggingManager.InsertErrorAsync(APP_NAME, exc);
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
                await _loggingManager.InsertErrorAsync(APP_NAME, exc);
                throw;
            }
        }

        [HttpPost]
        public async Task Save([FromBody] FinanceCategoryViewModel categoryViewModel)
        {
            try
            {
                if (categoryViewModel == null) throw new ArgumentNullException(nameof(categoryViewModel));

                var category = categoryViewModel.ToCategory();
                await _categorySaveWorkflow.SaveAsync(category).ConfigureAwait(true);
            }
            catch (Exception exc)
            {
                await _loggingManager.InsertErrorAsync(APP_NAME, exc).ConfigureAwait(true);
                throw;
            }
        }
        //public async Task Delete(string? id)
        //{
        //    try
        //    {
        //        await _appointmentService.DeleteAsync(id).ConfigureAwait(true);
        //    }
        //    catch (Exception exc)
        //    {
        //        await _logService.CreateErrorAsync(LogApplication.Appointments, exc).ConfigureAwait(true);
        //        throw;
        //    }
        //}
    }
}