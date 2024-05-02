using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.Links
{
    public interface ILinkManager
    {
        Task CreateOrUpdateLinkAsync(Link link);
        Task DeleteLinkAsync(Guid id);
        Task<IEnumerable<Link>> GetLinksAsync();
    }
}