using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CW1Preyanshu.Components.Model;
using CW1Preyanshu.Models;

public class UserService
{
    // Paths for storing application data
    private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
    private static readonly string FilePath = Path.Combine(FolderPath, "appdata.json");

    // Load AppData (Users, Transactions, Debts) from JSON file
    public AppData LoadData()
    {
        if (!File.Exists(FilePath))
        {
            return new AppData();
        }

        try
        {
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<AppData>(json) ?? new AppData();
        }
        catch (JsonException)
        {
            return new AppData();
        }
        catch (Exception)
        {
            return new AppData();
        }
    }

    // Save AppData (Users, Transactions, Debts) to JSON file
    public void SaveData(AppData data)
    {
        EnsureFolderExists();
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    // Manage Users within AppData
    public List<User> LoadUsers()
    {
        var appData = LoadData();
        return appData.Users;
    }

    public void SaveUsers(List<User> users)
    {
        var appData = LoadData();
        appData.Users = users;
        SaveData(appData);
    }

    // Set the user's preferred currency
    public void SetPreferredCurrency(string currency)
    {
        if (CurrentUser != null)
        {
            CurrentUser.PreferredCurrency = currency;
            SaveUsers(LoadUsers()); // Save the updated users list with the new currency
        }
    }

    // Get the preferred currency of the logged-in user
    public string GetPreferredCurrency()
    {
        return CurrentUser?.PreferredCurrency ?? "USD"; // Default to "USD" if no currency is set
    }

    // Hash a password securely
    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    // Validate a password against a stored hash
    public bool ValidatePassword(string inputPassword, string storedPassword)
    {
        var hashedInputPassword = HashPassword(inputPassword);
        return hashedInputPassword == storedPassword;
    }

    public User CurrentUser { get; private set; }

    // Utility: Ensure the data folder exists
    private void EnsureFolderExists()
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }
    }

    // Transaction Management
    public void AddTransaction(Transactions transaction)
    {
        var appData = LoadData();
        appData.Transactions.Add(transaction);
        SaveData(appData);
    }

    public void UpdateTransaction(Transactions updatedTransaction)
    {
        var appData = LoadData();
        var transaction = appData.Transactions.FirstOrDefault(t => t.Id == updatedTransaction.Id);
        if (transaction != null)
        {
            transaction.Debit = updatedTransaction.Debit;
            transaction.Credit = updatedTransaction.Credit;
            transaction.Description = updatedTransaction.Description;
            transaction.Note = updatedTransaction.Note;
            transaction.Tags = updatedTransaction.Tags;
            SaveData(appData);
        }
    }

    public void DeleteTransaction(int transactionId)
    {
        var appData = LoadData();
        var transaction = appData.Transactions.FirstOrDefault(t => t.Id == transactionId);
        if (transaction != null)
        {
            appData.Transactions.Remove(transaction);
            SaveData(appData);
        }
    }

    // Debt Management
    public void AddDebt(Debts debt)
    {
        var appData = LoadData();
        appData.Debts.Add(debt);
        SaveData(appData);
    }

    public void UpdateDebt(Debts updatedDebt)
    {
        var appData = LoadData();
        var debt = appData.Debts.FirstOrDefault(d => d.Id == updatedDebt.Id);
        if (debt != null)
        {
            debt.Amount = updatedDebt.Amount;
            debt.Description = updatedDebt.Description;
            SaveData(appData);
        }
    }


    public void RemoveDebt(int debtId)
    {
        var appData = LoadData();
        var debt = appData.Debts.FirstOrDefault(d => d.Id == debtId);
        if (debt != null)
        {
            appData.Debts.Remove(debt);
            SaveData(appData);
        }
    }

    // User Authentication
    public bool RegisterUser(User newUser)
    {
        var appData = LoadData();
        var existingUser = appData.Users.FirstOrDefault(u => u.Username == newUser.Username);
        if (existingUser != null) return false;

        newUser.Password = HashPassword(newUser.Password);
        appData.Users.Add(newUser);
        SaveData(appData);
        return true;
    }

    public User LoginUser(string username, string password)
    {
        var appData = LoadData();
        var user = appData.Users.FirstOrDefault(u => u.Username == username);
        if (user != null && ValidatePassword(password, user.Password))
        {
            CurrentUser = user;
            return user;
        }
        return null;
    }

    public void LogoutUser()
    {
        CurrentUser = null;
    }

    // Utility: Check if a user exists
    public bool UserExists(string username)
    {
        var appData = LoadData();
        return appData.Users.Any(u => u.Username == username);
    }
}
