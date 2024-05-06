namespace Net.Gwiasda.Links
{
    public interface ILinkRepository
    {
        Task CreateOrUpdateLinkAsync(Link link);
        Task DeleteLinkAsync(Guid id);
        Task<IEnumerable<Link>> GeLinksAsync();
    }
}