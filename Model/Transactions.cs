public class Transactions
{
    public int Id { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public string Note { get; set; }
    public string Type { get; set; }
    public string Source { get; set; }

    // Category property
    public string Category { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal RemainingDebt { get; set; }
    public string Currency { get; set; } // e.g., "USD"

    // New UserId property to link transaction with a specific user
    public int UserId { get; set; }

    // New DebtDueDate property to track the due date of the debt
    public DateTime? DebtDueDate { get; set; }  // Add this line

    // A helper property to handle tags as a string input, if necessary
    public string TagsString
    {
        get => string.Join(", ", Tags);
        set => Tags = value.Split(',').Select(t => t.Trim()).ToList();
    }

    // Set debit or credit based on transaction type
    public void SetAmount(decimal amount)
    {
        if (Type == "Debit")
        {
            Debit = amount;
        }
        else if (Type == "Credit")
        {
            Credit = amount;
        }
        else
        {
            throw new InvalidOperationException("Invalid transaction type. Use 'Debit' or 'Credit'.");
        }
    }

    // Method to format the Date
    public string GetFormattedDate()
    {
        return Date.ToString("MM/dd/yyyy");
    }

    // Method to update Remaining Debt
    public void UpdateRemainingDebt(decimal amountPaid)
    {
        RemainingDebt -= amountPaid;
        if (RemainingDebt < 0)
        {
            RemainingDebt = 0; // Prevent negative debt
        }
    }
}
