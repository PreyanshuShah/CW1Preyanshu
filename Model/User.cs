namespace CW1Preyanshu.Components.Model
{
    public class User
    {
        public int UserId { get; set; } // Unique ID for each user
        public string Username { get; set; }
        public string Password { get; set; } // This will store the hashed password
        public string Email { get; set; }

        public string PreferredCurrency { get; set; }
    }
}
