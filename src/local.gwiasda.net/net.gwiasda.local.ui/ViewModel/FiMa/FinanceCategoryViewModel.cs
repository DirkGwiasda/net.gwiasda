using Net.Gwiasda.FiMa;

namespace Net.Gwiasda.Local.UI.ViewModel.FiMa
{
    public class FinanceCategoryViewModel
    {
        public FinanceCategoryViewModel(FinanceCategory category)
        {
            Id = category.Id.ToString();
            ParentId = category.ParentId.ToString();
            Name = category.Name;
            Description = category.Description;
            Position = category.Position;
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}