using Net.Gwiasda.Links;

namespace Net.Gwiasda.Local.UI.ViewModel
{
    public class LinkViewModel
    {
        public LinkViewModel() { }
        public LinkViewModel(Link link)
        {
            Id = link.Id.ToString();
            Text = link.Text;
            Url = link.Url;
            Description = link.Description;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Text { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Link ToLink()
        { 
            if(!Guid.TryParse(Id, out Guid id))
                id = Guid.NewGuid();

            return new Link() { Description = Description, Id = id, Text = Text, Url = Url };
        }
    }
}