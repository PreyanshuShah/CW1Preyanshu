namespace CW1Preyanshu.Components.Model
{
    public class CurrencyService
    {
        private string _preferredCurrency = "NPR"; // Default to NPR (Nepalese Rupee)

        // Method to get the available currencies
        public List<string> GetAvailableCurrencies()
        {
            return new List<string> { "USD", "NPR" }; // Only allow USD and NPR
        }

        // Method to get the preferred currency
        public string GetPreferredCurrency()
        {
            return _preferredCurrency; // Return the stored preferred currency
        }

        // Method to set a new preferred currency
        public void SetPreferredCurrency(string currency)
        {
            _preferredCurrency = currency; // Save the selected currency
        }

        // Method to convert an amount from NPR to USD and vice versa
        public decimal ConvertCurrency(decimal amount)
        {
            if (_preferredCurrency == "USD")
            {
                return amount * 0.0076923m; // Convert NPR to USD (1 NPR = 0.0076923 USD)
            }
            else if (_preferredCurrency == "NPR")
            {
                return amount * 130m; // Convert USD to NPR (1 USD = 130 NPR)
            }
            return amount; // Return the same amount if currencies are the same
        }
    }
}
