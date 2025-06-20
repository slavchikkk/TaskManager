using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManager
{
    // Основной класс программы
    class Program
    {
        // Глобальные списки задач и пользователей
        // static List<Task> Tasks = new List<Task>();
        // static List<User> Users = new List<User>();
        static UserStorage userStorage = UserStorage.getInstance();
        static TaskStorage taskStorage = TaskStorage.getInstance();
        
        static void Main(string[] args)
        {
            // Основная логика программы
            while (true)
            {
                ShowMenu();

                try
                {
                    var choice = int.Parse(Console.ReadLine());

                    ProcessChoice(choice);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка: введено недопустимое значение.");
                }
            }
        }

        // Показываем главное меню
        static void ShowMenu()
        {
            Console.WriteLine("\nСписок действий:");
            Console.WriteLine("1. Создать задачу");
            Console.WriteLine("2. Создать пользователя");
            Console.WriteLine("3. Назначить ответственного");
            Console.WriteLine("4. Добавить комментарий");
            Console.WriteLine("5. Изменить статус задачи");
            Console.WriteLine("6. Просмотреть прогресс");
            Console.WriteLine("7. Отсортировать задачи");
            Console.WriteLine("8. Генерация отчета");
            Console.WriteLine("9. Добавить наблюдателя к задаче");
            Console.WriteLine("10. Убрать наблюдателя из задачи");
            Console.WriteLine("0. Выход");
        }

        // Выполняем выбранное действие
        static void ProcessChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    CreateTask();
                    break;
                case 2:
                    CreateUser();
                    break;
                case 3:
                    AssignExecutor();
                    break;
                case 4:
                    AddComment();
                    break;
                case 5:
                    ChangeTaskStatus();
                    break;
                case 6:
                    TrackProgress();
                    break;
                case 7:
                    FilterTasks();
                    break;
                case 8:
                    GenerateReport();
                    break;
                case 9:
                    AddSpectatorInTask();
                    break;
                case 10:
                    DeleteSpectatorFromTask();
                    break;
                case 0:
                    ExitApp();
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Повторите попытку.");
                    break;
            }
        }

        // Создание новой задачи
        static void CreateTask()
        {
            Console.Write("Введите название задачи: ");
            string taskName = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(taskName))
            {
                Console.WriteLine("Название задачи не может быть пустым! Повторите создание задачи.");
                return;
            }

            Console.Write("Введите срок выполнения (гггг-мм-дд): ");
            DateTime taskDeadline;
            bool isValidDate = DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out taskDeadline);

            if (!isValidDate)
            {
                Console.WriteLine("Срок введен неправильно. Повторите создание задачи.");
                return;
            }

            Console.Write("Укажите приоритет задачи: ");
            string taskPriority = Console.ReadLine().Trim();
            
            Task newTask = new Task(taskStorage.GetNextId(), taskName, taskDeadline, taskPriority);
            // Tasks.Add(newTask);
            taskStorage.AddTask(newTask);
            Console.WriteLine("Задача успешно создана!");
        }
        
        static void ChangeTaskStatus()
        {
            if (taskStorage.GetTasks().Count == 0)
            {
                Console.WriteLine("Нет активных задач");
                return;
            }

            Task currentTask = GetUserInputTask();
            TaskProgress status = GetUserInputTaskProgress();

            string progress = "";
            switch (status)
            {
                case TaskProgress.New:
                    progress = "Новая";
                    break;
                case TaskProgress.InProgress:
                    progress = "В работе";
                    break;
                case TaskProgress.Completed:
                    progress = "Завершена";
                    break;
                case TaskProgress.Cancelled:
                    progress = "Отменена";
                    break;

            }
            Console.WriteLine($"Статус задачи {currentTask.Name} изменен. Текущий статус: {progress}");

            EmailService emailService = new EmailService("smtp.yandex.ru", 587, "m1knll@yandex.ru", "kqsncbjurngfscnl");
            List<string> emailList = new List<string>();

            if (currentTask.Spectators == null)
            {
                currentTask.Spectators = new List<User>();
            }
            
            // Проходим по списку наблюдателей
            foreach (var user in currentTask.Spectators)
            {
                // если почта заполнена - добавляем в список
                if (!string.IsNullOrEmpty(user.Email))
                {
                    emailList.Add(user.Email);        
                }
            }
            // если у исполнителя заполнена почта - добавляем в список
            if (!string.IsNullOrEmpty(currentTask.Executor.Email))
            {
                emailList.Add(currentTask.Executor.Email);
            }

            string subject = $"Изменения по задаче {currentTask.Id}";
            string body = $"В задаче {currentTask.Id} изменился статус. Текущий статус: {progress}";
            emailService.SendEmail(emailList, body, subject);
        }

        static void AddSpectatorInTask()
        {
            Console.WriteLine("В какую задачу хотите добавить наблюдателя?");
            Task currentTask = GetUserInputTask();

            Console.WriteLine("Кого добавить в задачу?");
            User currentUser = GetUserInputUser();

            if (currentUser.Id == currentTask.Executor.Id)
            {
                Console.WriteLine("Нельзя добавить исполнителя в наблюдатели. Повторите попытку.");
                return;
            }
            
            if (currentTask.Spectators == null)
            {
                currentTask.Spectators = new List<User>();
            }
            
            if (currentTask.Spectators.Any(user => user.Id == currentUser.Id))
            {
                Console.WriteLine("Наблюдатель уже добавлен в задачу. Повторите попытку.");
                return;
            }

        currentTask.Spectators.Add(currentUser);
        }

        static void DeleteSpectatorFromTask()
        {
            Console.WriteLine("Откуда хотите удалить наблюдателя?");
            Task currentTask = GetUserInputTask();
            
            int answer = 0;
            while (true)
            {
                if (currentTask.Spectators == null || currentTask.Spectators.Count == 0)
                {
                    Console.WriteLine("Наблюдателей нет.");
                    return;
                }
                
                Console.WriteLine("Введите ИД пользователя");
                
                // Проходим по списку наблюдателей и выводим их
                foreach (var currentTaskSpectator in currentTask.Spectators)
                {
                    Console.WriteLine($"ИД: {currentTaskSpectator.Id}. Имя: {currentTaskSpectator.Name}");
                }
                // Получаем ИД наблюдателя
                if (int.TryParse(Console.ReadLine(), out answer))
                {
                    // Проверяем есть ли он в списке
                    if (currentTask.Spectators.Any(spectator => spectator.Id == answer))
                    {
                        // Удаляем
                        currentTask.Spectators.Remove(currentTask.Spectators.First(sp => sp.Id == answer));
                        Console.WriteLine($"Пользователь с ИД {answer} удален из наблюдателей.");
                        return;
                    }

                    Console.WriteLine("Данного ИД в списке нет. Повторите попытку.");
                    return;
                }

                Console.WriteLine("Ошибка. Повторите попытку.");
                return;
            }
        }

        static void CreateUser()
        {
            Console.Write("Введите ФИО пользователя: ");
            string userName = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(userName))
            {
                Console.WriteLine("Имя пользователя не может быть пустым.");
                return;
            }

            Console.Write("Введите email пользователя: ");
            string email = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email пользователя не может быть пустым.");
            }

            User newUser = new User(userName, email);
            // Users.Add(newUser);
            userStorage.AddUser(newUser);
            Console.WriteLine("Пользователь успешно создан!");
        }

        // Назначение исполнителя задачи
        static void AssignExecutor()
        {
            if (taskStorage.GetTasks().Count == 0)
            {
                Console.WriteLine("Нет активных задач");
                return;
            }

            if (userStorage.GetUsers().Count == 0)
            {
                Console.WriteLine("Нет активных пользователей");
                return;
            }

            Task currentTask = GetUserInputTask();
            User currentUser = GetUserInputUser();

            
            taskStorage.GetTasks()[currentTask.Id - 1].Executor = currentUser;
          
            Console.WriteLine($"Ответственный {currentUser.Name} успешно назначен на задачу {currentTask.Name}");
        }


        // Получение Task от пользователя

        static Task GetUserInputTask()
        {
            Console.WriteLine("Укажите ИД задачи: ");

            // Выводим список задач для удобного ввода данных.
            foreach (Task task in taskStorage.GetTasks())
            {
                Console.WriteLine($"Наименование: {task.Name}, ИД: {task.Id}");
            }

            if (int.TryParse(Console.ReadLine(), out int taskId))
            {
                // Проверяем что ИД задачи входит в список.
                if (taskStorage.GetTasks().Count >= taskId && taskId > 0)
                {
                    taskId -= 1; // вычитаем 1 чтобы получить позицию в листе.
                    return taskStorage.GetTasks()[taskId];
                }
                else
                {
                    Console.WriteLine("Введен неверный ИД. Повторите попытку.");
                    return GetUserInputTask();
                }
            }
            else
            {
                Console.WriteLine("Введен неверный ИД. Повторите попытку.");
                return GetUserInputTask();
            }
        }
        
        // Получение User от пользователя
        static User GetUserInputUser()
        {
            Console.WriteLine("Укажите ИД пользователя: ");

            // Выводим список пользователей для удобного ввода данных.
            foreach (User user in userStorage.GetUsers())
            {
                Console.WriteLine($"Имя: {user.Name}, ИД: {user.Id}");
            }

            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                foreach (User user in userStorage.GetUsers())
                {
                    if (user.Id == userId)
                    {
                        return user;
                    }
                }
                {
                    Console.WriteLine("Введен неверный ИД. Повторите попытку.");
                    return GetUserInputUser();
                }
            }
            else
            {
                Console.WriteLine("Введен неверный ИД. Повторите попытку.");
                return GetUserInputUser();
            }
        }
        
        static TaskProgress GetUserInputTaskProgress()
        {
            Console.WriteLine("Выберете на какой статус поменять.");
            Console.WriteLine("1. Новая.");
            Console.WriteLine("2. В работе.");
            Console.WriteLine("3. Завершено.");
            Console.WriteLine("4. Отменено.");

            if (!int.TryParse(Console.ReadLine(), out int status))
            {
                Console.WriteLine("Ошибка! Введите корректное значение.");
                return GetUserInputTaskProgress();
            }

            switch (status)
            {
                case 1:
                    return TaskProgress.New;
                case 2:
                    return TaskProgress.InProgress;
                case 3:
                    return TaskProgress.Completed;
                case 4:
                    return TaskProgress.Cancelled;
                default:
                    Console.WriteLine("Ошибка! Введите корректное значение.");
                    return GetUserInputTaskProgress();

            }
        }

        // Отслеживаем прогресс задач
        static void TrackProgress()
        {
            if (taskStorage.GetTasks().Count == 0)
            {
                Console.WriteLine("Нет активных задач");
                return;
            }

            Task currentTask = GetUserInputTask();
            switch (currentTask.Progress)
            {
                case TaskProgress.New:
                    Console.WriteLine($"Статус задачи {currentTask.Name}: новая. Крайний срок {currentTask.Deadlines}");
                    break;
                case TaskProgress.InProgress:
                    Console.WriteLine($"Статус задачи {currentTask.Name}: в работе. Крайний срок {currentTask.Deadlines}");
                    break;
                case TaskProgress.Completed:
                    Console.WriteLine($"Статус задачи {currentTask.Name}: завершена. Крайний срок {currentTask.Deadlines}");
                    break;
                case TaskProgress.Cancelled:
                    Console.WriteLine($"Статус задачи {currentTask.Name}: отменена. Крайний срок {currentTask.Deadlines}");
                    break;
            }
        }

        // Добавляем комментарии к задаче
        static void AddComment()
        {
            if (taskStorage.GetTasks().Count == 0)
            {
                Console.WriteLine("Нет активных задач");
                return;
            }

            Console.Write("Выберите номер задачи для комментирования: ");
            int taskIndex = int.Parse(Console.ReadLine()) - 1;

            if (taskIndex >= 0 && taskIndex < taskStorage.GetTasks().Count)
            {
                Console.Write("Ваш комментарий: ");
                string comment = Console.ReadLine();
                taskStorage.GetTasks()[taskIndex].Comment = comment;
                Console.WriteLine("Комментарий добавлен успешно!");
            }
            else
            {
                Console.WriteLine("Указанная задача не найдена.");
            }
        }


        // Сортировка задач

        static void FilterTasks()
        {
            if (taskStorage.GetTasks().Count == 0)
            {
                Console.WriteLine("Нет активных задач");
                return;
            }

            Console.WriteLine("\nОтсортировать задачи:");
            Console.WriteLine("1. По номеру");
            Console.WriteLine("2. По сроку");
            Console.WriteLine("3. По приоритету");
            Console.Write("Выберите критерий: ");

            try
            {
                int filternumber = int.Parse(Console.ReadLine());

                switch (filternumber)
                {
                    case 1:
                        FilterId();
                        break;
                    case 2:
                        FilterDeadline();
                        break;
                    case 3:
                        FilterPriority();
                        break;
                    default:
                        Console.WriteLine("Неправильный выбор критерия сортировки");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: введено недопустимое значение");
            }
        }

        static void FilterId()
        {
            IEnumerable<Task> sortedTasks = taskStorage.GetTasks().OrderBy(t => t.Id);
            DisplaySortedList(sortedTasks);
        }

        static void FilterDeadline()
        {
            IEnumerable<Task> sortedTasks = taskStorage.GetTasks().OrderBy(t => t.Deadlines);
            DisplaySortedList(sortedTasks);
        }

        static void FilterPriority()
        {
            IEnumerable<Task> sortedTasks = taskStorage.GetTasks().OrderBy(t => t.Priority);
            DisplaySortedList(sortedTasks);
        }

        static void DisplaySortedList(IEnumerable<Task> TasksToDisplay)
        {
            foreach (var task in TasksToDisplay)
            {
                Console.WriteLine($"#{task.Id}. Название: {task.Name}, Срок: {task.Deadlines.ToString("yyyy-MM-dd")}, Приоритет: {task.Priority}");
            }
        }



        // Генерация отчета по задачам
        static void GenerateReport()
        {
            Console.WriteLine("Отчет надо реализовывать");
        }


        // Завершение программы
        static void ExitApp()
        {
            taskStorage.SaveAllToFile();
            Environment.Exit(0);
        }
    }
}

