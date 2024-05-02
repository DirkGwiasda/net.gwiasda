using Microsoft.AspNetCore.Mvc;
using Net.Gwiasda.FiMa;
using Net.Gwiasda.Links;
using Net.Gwiasda.Local.UI.ViewModel;
using Net.Gwiasda.Local.UI.ViewModel.FiMa;
using Net.Gwiasda.Logging;

namespace Net.Gwiasda.Local.UI.Controllers
{
    public class LinkController : Controller
    {
        private const string APP_NAME = "Links";
        private readonly ILoggingManager _loggingManager;
        private readonly ILinkManager _linkManager;

        public LinkController(ILoggingManager loggingManager, ILinkManager linkManager)
        {
            _loggingManager = loggingManager ?? throw new ArgumentNullException(nameof(loggingManager));
            _linkManager = linkManager ?? throw new ArgumentNullException(nameof(linkManager));
        }

        public Task<string> Ping()
        {
            return Task.FromResult($"{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")} Pong from LinksController");
        }

        public async Task<IEnumerable<LinkViewModel>> GetLinks()
        {
            try
            {
                return (await _linkManager.GetLinksAsync()).Select(link => new LinkViewModel(link));
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc);
                throw;
            }
        }
        [HttpPost]
        public async Task Save([FromBody] LinkViewModel linkViewModel)
        {
            try
            {
                if (linkViewModel == null) throw new ArgumentNullException(nameof(linkViewModel));
                var link = linkViewModel.ToLink();

                await _linkManager.CreateOrUpdateLinkAsync(link);
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc);
                throw;
            }
        }
        public async Task Delete(string? id)
        {
            try
            {
                if(!Guid.TryParse(id, out Guid linkId))
                    throw new InvalidCastException($"Cannot parse link id '{id}'.");

                await _linkManager.DeleteLinkAsync(linkId);
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(APP_NAME, exc).ConfigureAwait(true);
                throw;
            }
        }
    }
}