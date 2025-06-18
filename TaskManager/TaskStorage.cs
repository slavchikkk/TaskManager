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
        File.AppendAllText(FilePath, task.ToDataString() + Environment.NewLine);
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
                .Select(FromDataString)
                .ToList();
        }
    }
    
    private int GetNextId()
    {
        return tasks.Count + 1;
    }
    
    public static Task FromDataString(string data)
    {
        var splittedStrings = data.Split('|');
        DateTime deadLine;
        DateTime.TryParseExact(splittedStrings[2], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out deadLine);
        return new Task(int.Parse(splittedStrings[0]), splittedStrings[1], deadLine, splittedStrings[3], splittedStrings[4]);
    }
}