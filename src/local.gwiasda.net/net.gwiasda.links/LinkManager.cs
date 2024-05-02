using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.Links
{
    public class LinkManager : ILinkManager
    {
        private readonly ILinkValidator _validator;
        private readonly ILinkRepository _repository;

        public LinkManager(ILinkValidator validator, ILinkRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task CreateOrUpdateCategoryAsync(Link link)
        {
            _validator.Validate(link);
            await _repository.CreateOrUpdateCategoryAsync(link);
        }

        public async Task DeleteCategoryAsync(Guid id)
            => await _repository.DeleteCategoryAsync(id);

        public Task<IEnumerable<Link>> GetCategoriesAsync()
            => _repository.GetCategoriesAsync();
    }
}