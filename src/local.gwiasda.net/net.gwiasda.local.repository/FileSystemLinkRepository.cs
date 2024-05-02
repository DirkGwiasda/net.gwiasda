using Net.Gwiasda.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Net.Gwiasda.Local.Repository
{
    public class FileSystemLinkRepository : ILinkRepository
    {
        internal const string FileName = "links.dat";

        private readonly object _sync = new object();

        public Task CreateOrUpdateCategoryAsync(Link link)
        {
            lock(_sync)
            {
                var links = GetCategoriesAsync().Result.ToList();
                var existing = links.FirstOrDefault(l => l.Id == link.Id);
                if(existing != null)
                    links.Remove(existing);

                links.Add(link);

                SaveCategories(links);
            }
            return Task.CompletedTask;
        }

        public Task DeleteCategoryAsync(Guid id)
        {
            lock (_sync)
            {
                var links = GetCategoriesAsync().Result.ToList();
                var existing = links.FirstOrDefault(l => l.Id == id);
                if (existing != null)
                {
                    links.Remove(existing);
                    SaveCategories(links);
                }
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Link>> GetCategoriesAsync()
        {
            var fileName = GetCostCategoriesFileName();
            if(!File.Exists(fileName))
                return Enumerable.Empty<Link>();

            var json = await File.ReadAllTextAsync(GetCostCategoriesFileName());
            if(string.IsNullOrWhiteSpace(json))
                return Enumerable.Empty<Link>();

            return JsonSerializer.Deserialize<IEnumerable<Link>>(json) ?? Enumerable.Empty<Link>();
        }

        private void SaveCategories(IEnumerable<Link> links)
        {
            var json = JsonSerializer.Serialize(links);
            File.WriteAllText(GetCostCategoriesFileName(), json);
        }

        private string GetCostCategoriesFileName() => Path.Combine(FileSystemRepository.RootDataDirectory, FileName);
    }
}