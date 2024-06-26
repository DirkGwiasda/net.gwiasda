﻿namespace Net.Gwiasda
{
    public class Booking
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Text { get; set; } = string.Empty;
        public Guid CategoryId { get; set; } = Guid.Empty;
        public bool IsCost { get; set; }
        public decimal Amount { get; set; }
    }
}