using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace TaskManager;

class Program
{
    static List<Task> CreateTask(List<Task> ListOfTasks)
    {
        bool ContinueRequest = true;
        while (ContinueRequest)
        {
            Console.Write("\n Введите название задачи: ");
            string TaskName = Console.ReadLine()!;

            bool TaskExist = false;
            foreach (Task task in ListOfTasks)
            {
                if (task.Name != null &&
                    task!.Name == TaskName)
                {
                    Console.WriteLine("Задача с таким названием уже существует");
                    TaskExist = true;
                    break;
                }
            }
            if (!TaskExist)
            {
                Console.Write("\n Введите описание задачи: ");
                string TaskDescription = Console.ReadLine()!;

                DateOnly TaskDeadlines;
                bool isValid = false;
                do
                {
                    Console.Write("Введите срок выполнения задачи (в формате ДД.ММ.ГГГГ): ");
                    string input = Console.ReadLine()!;

                    if (DateOnly.TryParseExact(input, "dd.MM.yyyy", null,
                            System.Globalization.DateTimeStyles.None, out TaskDeadlines))
                    {
                        isValid = true;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный формат даты. Попробуйте снова.");
                    }

                } while (!isValid);

                Console.Write("\n Выберите приоритет задачи и введите его номер(1.Наивысший, 2. Средний, 3.Низсший): ");

                string TaskPriority = "";
                isValid = false;
                do
                {
                    switch (Console.ReadLine())
                    {
                        case "1":
                            {
                                TaskPriority = "Наивысший";
                                isValid = true;
                                break;
                            }
                        case "2":
                            {
                                TaskPriority = "Средний";
                                isValid = true;
                                break;
                            }
                        case "3":
                            {
                                TaskPriority = "Низший";
                                isValid = true;
                                break;
                            }
                        default:
                            Console.WriteLine("Введено неверное значение, повторите ввод" + "\n");
                            break;
                    }
                } while (!isValid);
                ListOfTasks.Add(new Task(TaskName, TaskDeadlines, TaskPriority, TaskDescription));
            }
            string UserAnswer;
            do
            {
                Console.WriteLine("Хотите ли вы добавить нового абонента? Y/N");
                UserAnswer = Console.ReadLine()!.ToUpper();
            } while (UserAnswer != "Y" && UserAnswer != "N");
            ContinueRequest = (UserAnswer == "Y");
        }

        Console.WriteLine();
        return ListOfTasks;
    }

    static void ShowListOfTasks(List<Task> ListOfTasks)
    {
        foreach (Task task in ListOfTasks)
        {
            Console.WriteLine($"{task.Id}. {task.Name}");
            Console.WriteLine($"Описание : {task.Description}");
            Console.WriteLine($"Сроки выполнения задачи: {task.Deadlines}");
            Console.WriteLine($"Приоритет выполнения задачи: {task.Priority}");
            Console.WriteLine($"Ответственный за выполнение задачи: {task.Executor.Name}");
        }
    }

    static void Main(string[] args)
    {
        List<User> ListOfUsers = [];
        List<Task> ListOfTasks = [];

        if (ListOfUsers.Count != 0)
        {
            int currentMaxIdUsers = ListOfUsers.Max(x => x.Id);
            User.SetNextId(currentMaxIdUsers + 1);
        }

        if (ListOfTasks.Count != 0)
        {
            int currentMaxIdTasks = ListOfTasks.Max(x => x.Id);
            Task.SetNextId(currentMaxIdTasks + 1);
        }

        CreateTask(ListOfTasks);
        ShowListOfTasks(ListOfTasks);
    }
}