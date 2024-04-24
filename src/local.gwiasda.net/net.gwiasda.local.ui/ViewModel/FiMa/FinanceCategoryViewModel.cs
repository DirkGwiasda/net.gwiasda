using Net.Gwiasda.FiMa;

namespace Net.Gwiasda.Local.UI.ViewModel.FiMa
{
    public class FinanceCategoryViewModel
    {
        public FinanceCategoryViewModel() { }
        public FinanceCategoryViewModel(FinanceCategory category)
        {
            Id = category.Id.ToString();
            ParentId = category.ParentId.ToString();
            Name = category.Name;
            Description = category.Description;
            Position = category.Position;
            IsCostCategory = category is CostCategory;
            Hierarchy = category.Hierarchy;
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Position { get; set; }
        public bool IsCostCategory { get; set; }
        public int Hierarchy { get; set; }

        public FinanceCategory ToCategory()
        {
            var id = string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid() : Guid.Parse(Id);
            var parentId = string.IsNullOrWhiteSpace(ParentId) ? (Guid?)null : Guid.Parse(ParentId);

            FinanceCategory category = IsCostCategory ? new CostCategory() : new IncomeCategory();
            category.Id = id;
            category.ParentId = parentId;
            category.Name = Name;
            category.Description = Description;
            category.Position = Position;
            category.Hierarchy = Hierarchy;

            return category;
        }
    }
}