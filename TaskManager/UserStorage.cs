namespace TaskManager;

/// <summary>
/// Синглтон класс хранения пользователей
/// </summary>
public class UserStorage
{
    private static UserStorage instance;
    private const string FilePath = "../../users.txt";
    private List<User> users;
    private UserStorage()
    {
        CreateIfNotExistsFile();
        LoadUsersFromFile();
    }

    public static UserStorage getInstance()
    {
        if (instance == null)
            instance = new UserStorage();
        return instance;
    }

    public void AddUser(User user)
    {
        if (IsEmailUsed(user.Email)) return;
        user.Id = GetNextId();
        users.Add(user);
        File.AppendAllText(FilePath, user.ToDataString() + Environment.NewLine);
    }
    
    private void CreateIfNotExistsFile()
    {
        if (!File.Exists(FilePath))
        {
            File.Create(FilePath).Close();
        }
    }
    
    private void LoadUsersFromFile()
    {
        if (!File.Exists(FilePath))
        {
            users = new List<User>();
        }
        else
        {
            users = File.ReadAllLines(FilePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(FromDataString)
                .ToList();
        }
    }
    
    private int GetNextId()
    {
        return users.Count + 1;
    }
    
    public static User FromDataString(string data)
    {
        var splittedStrings = data.Split('|');
        return new User(int.Parse(splittedStrings[0]), splittedStrings[1], splittedStrings[2]);
    }
    
    public bool IsEmailUsed(string email)
    {
        return users.Any(a => a.Email == email);
    }
}