namespace TaskManager;

public class User
{
    //Поле для отслеживания ID
    private static int NextId = 1;

    public static void SetNextId(int NewId)
    {
        NextId = NewId;
    }

    /// <summary>
    /// ИД пользователя
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="id">ИД</param>
    /// <param name="name">Имя</param>
    /// <param name="email">Email</param>
    public User(int id, string name, string email)
    {
        this.Id = NextId++;
        this.Name = name;
        this.Email = email;
    }

    public User(string name, string email) : this (0, name, email) { }
    
    public User(string name) : this(0, name, String.Empty) { }

    public static User GetDefaultUser()
    {
        return new User("Исполнитель не назначен");
    }
}