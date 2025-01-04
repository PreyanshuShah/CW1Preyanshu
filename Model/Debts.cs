using System;
using System.ComponentModel.DataAnnotations;

namespace CW1Preyanshu.Models
{
    public class Debts
    {
        public int Id { get; set; }

        public int UserId { get; set; } // Link debt to a user

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; set; } // Total debt amount

        [Range(0, double.MaxValue, ErrorMessage = "Paid amount must be a positive value.")]
        public decimal PaidAmount { get; set; } = 0; // Amount paid towards debt

        public DateTime Date { get; set; }

        [StringLength(500, ErrorMessage = "Description should not exceed 500 characters.")]
        public string Description { get; set; }

        // Computed property for remaining debt
        public decimal RemainingDebt => Amount - PaidAmount;
    }
}
