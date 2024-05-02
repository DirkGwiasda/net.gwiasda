using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.Links
{
    public interface ILinkRepository
    {
        Task CreateOrUpdateCategoryAsync(Link link);
        Task DeleteCategoryAsync(Guid id);
        Task<IEnumerable<Link>> GetCategoriesAsync();
    }
}