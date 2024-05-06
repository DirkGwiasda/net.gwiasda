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

        public Task CreateOrUpdateLinkAsync(Link link)
        {
            lock(_sync)
            {
                var links = GeLinksAsync().Result.ToList();
                var existing = links.FirstOrDefault(l => l.Id == link.Id);
                if(existing != null)
                    links.Remove(existing);

                links.Add(link);

                SaveLinks(links);
            }
            return Task.CompletedTask;
        }

        public Task DeleteLinkAsync(Guid id)
        {
            lock (_sync)
            {
                var links = GeLinksAsync().Result.ToList();
                var existing = links.FirstOrDefault(l => l.Id == id);
                if (existing != null)
                {
                    links.Remove(existing);
                    SaveLinks(links);
                }
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Link>> GeLinksAsync()
        {
            var fileName = GetLinksFileName();
            if(!File.Exists(fileName))
                return Enumerable.Empty<Link>();

            var json = await File.ReadAllTextAsync(GetLinksFileName());
            if(string.IsNullOrWhiteSpace(json))
                return Enumerable.Empty<Link>();

            return JsonSerializer.Deserialize<IEnumerable<Link>>(json) ?? Enumerable.Empty<Link>();
        }

        private void SaveLinks(IEnumerable<Link> links)
        {
            if (!Directory.Exists(FileSystemRepository.RootDataDirectory))
                Directory.CreateDirectory(FileSystemRepository.RootDataDirectory);

            var json = JsonSerializer.Serialize(links);
            File.WriteAllText(GetLinksFileName(), json);
        }

        private string GetLinksFileName() => Path.Combine(FileSystemRepository.RootDataDirectory, FileName);
    }
}