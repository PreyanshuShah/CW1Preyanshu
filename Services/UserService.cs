using CW1Preyanshu.Components.Model;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Linq;
using CW1Preyanshu.Models;

public class UserService
{
    private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
    private static readonly string UsersFilePath = Path.Combine(FolderPath, "users.json");

    public User CurrentUser { get; private set; }
    public AppData CurrentUserData { get; private set; }

    // Set the current user and load their data
    public void SetCurrentUser(User user)
    {
        CurrentUser = user;
        CurrentUserData = LoadUserData(user.UserId); // Load user-specific data
    }

    // Load users from the file
    public List<User> LoadUsers()
    {
        if (!File.Exists(UsersFilePath))
        {
            return new List<User>();
        }

        try
        {
            var json = File.ReadAllText(UsersFilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
        catch (JsonException)
        {
            return new List<User>();
        }
    }

    // Save users to the file
    public void SaveUsers(List<User> users)
    {
        EnsureFolderExists();
        var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(UsersFilePath, json);
    }

    // Register a new user
    public bool RegisterUser(User newUser)
    {
        var users = LoadUsers();
        if (users.Any(u => u.Username == newUser.Username))
        {
            return false; // User already exists
        }

        newUser.UserId = users.Count > 0 ? users.Max(u => u.UserId) + 1 : 1;
        newUser.Password = HashPassword(newUser.Password);
        users.Add(newUser);
        SaveUsers(users);

        var userData = new AppData(); // Empty data for the new user
        SaveUserData(newUser.UserId, userData); // Save their empty data

        return true;
    }

    // Login and set current user
    public User LoginUser(string username, string password)
    {
        var users = LoadUsers();
        var user = users.FirstOrDefault(u => u.Username == username);
        if (user != null && ValidatePassword(password, user.Password))
        {
            SetCurrentUser(user); // Set the current user
            return user;
        }
        return null; // Invalid login
    }

    // Logout and clear current user data
    public void LogoutUser()
    {
        CurrentUser = null;
        CurrentUserData = null;
    }

    // Load user-specific data from their file
    private AppData LoadUserData(int userId)
    {
        var userFilePath = GetUserFilePath(userId);
        if (!File.Exists(userFilePath))
        {
            Console.WriteLine($"No data found for user {userId}. Creating new data.");
            return new AppData(); // Return default data if no file exists
        }

        try
        {
            var json = File.ReadAllText(userFilePath);
            return JsonSerializer.Deserialize<AppData>(json) ?? new AppData(); // Deserialize user data
        }
        catch (JsonException)
        {
            Console.WriteLine($"Error loading data for user {userId}. Creating new data.");
            return new AppData(); // Handle deserialization errors
        }
    }

    // Save user data to their specific file
    private void SaveUserData(int userId, AppData data)
    {
        EnsureFolderExists();
        var userFilePath = GetUserFilePath(userId);
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(userFilePath, json); // Save user data
    }

    // Get file path for the user-specific data
    private string GetUserFilePath(int userId)
    {
        return Path.Combine(FolderPath, $"user_{userId}.json"); // File for each user
    }

    // Ensure the folder exists for storing data
    private void EnsureFolderExists()
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath); // Make sure the folder exists
        }
    }

    // Save the current user's data
    public void SaveData(AppData data)
    {
        if (CurrentUser != null)
        {
            SaveUserData(CurrentUser.UserId, data); // Save data for current user
        }
    }

    // Hash password
    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash); // Return hashed password
    }

    // Validate the password
    public bool ValidatePassword(string inputPassword, string storedPassword)
    {
        var hashedInputPassword = HashPassword(inputPassword);
        return hashedInputPassword == storedPassword; // Compare hashed passwords
    }

    // Load current user's data
    public AppData LoadData()
    {
        return CurrentUserData; // Return data for current user
    }
}
