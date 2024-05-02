namespace Net.Gwiasda.Links
{
    public class LinkValidator : ILinkValidator
    {
        public void Validate(Link? link)
        {
            if(link == null) throw new ArgumentNullException(nameof(link));

            if (string.IsNullOrWhiteSpace(link.Text)) throw new ArgumentException("Text must not be empty", nameof(link.Text));
            if (string.IsNullOrWhiteSpace(link.Url)) throw new ArgumentException("Url must not be empty", nameof(link.Url));

            if(!Uri.TryCreate(link.Url, UriKind.Absolute, out var uri))
            {
                throw new ArgumentException("Url must be a valid absolute uri", nameof(link.Url));
            }
        }
    }
}