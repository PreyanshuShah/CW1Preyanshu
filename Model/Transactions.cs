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
    public DateTime? DueDate { get; set; }
    public decimal RemainingDebt { get; set; }

    // New UserId property to link transaction with a specific user
    public int UserId { get; set; }

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
            Debit = amount;
        else if (Type == "Credit")
            Credit = amount;
    }
}
