namespace TaskManager;

public class User
{
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
        this.Id = id;
        this.Name = name;
        this.Email = email;
    }
}