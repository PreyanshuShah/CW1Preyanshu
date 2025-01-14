namespace CW1Preyanshu.Components.Model
{
    public class Debts
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Link debt to a specific user
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Currency { get; set; } // e.g., "USD"
    }
}
