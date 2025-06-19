using System.Text.Json;

namespace TaskManager;

/// <summary>
/// Синглтон класс хранения задач
/// </summary>
public class TaskStorage
{
    private static TaskStorage instance;
    private const string FilePath = "../../tasks.txt";
    private List<Task> tasks;
    private TaskStorage()
    {
        CreateIfNotExistsFile();
        LoadTasksFromFile();
    }

    public static TaskStorage getInstance()
    {
        if (instance == null)
            instance = new TaskStorage();
        return instance;
    }
    
    public void AddTask(Task task)
    {
        task.Id = GetNextId();
        tasks.Add(task);
        File.AppendAllText(FilePath, SerializeData(task) + Environment.NewLine);
    }
    
    private void CreateIfNotExistsFile()
    {
        if (!File.Exists(FilePath))
        {
            File.Create(FilePath).Close();
        }
    }
    
    private void LoadTasksFromFile()
    {
        if (!File.Exists(FilePath))
        {
            tasks = new List<Task>();
        }
        else
        {
            tasks = File.ReadAllLines(FilePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(DeserializeTask)
                .ToList();
        }
    }
    
    public int GetNextId()
    {
        return tasks.Count + 1;
    }

    public List<Task> GetTasks()
    {
        return tasks;
    }
    
    public static string SerializeData(Task task)
    {
        return JsonSerializer.Serialize(task);
    }

    public static Task DeserializeTask(string serializedTask)
    {
        return JsonSerializer.Deserialize<Task>(serializedTask);
    }

    public void SaveAllToFile()
    {
        File.WriteAllLines(FilePath, tasks.Select(task => SerializeData(task) + Environment.NewLine));
    }
}