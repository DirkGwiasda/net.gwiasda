﻿namespace Net.Gwiasda.FiMa
{
    public abstract class FinanceCategory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Position { get; set; }
        public int Hierarchy { get; set; }
    }
}