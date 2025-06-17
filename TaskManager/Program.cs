using System;
using System.Collections.Generic;

namespace TaskManager
{
    // Основной класс программы
    class Program
    {
        // Глобальные списки задач и пользователей
        static List<Task> Tasks = new List<Task>();
        static List<User> Users = new List<User>();

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
            Console.WriteLine("2. Назначить ответственного");
            Console.WriteLine("3. Просмотреть прогресс");
            Console.WriteLine("4. Добавить комментарий");
            Console.WriteLine("5. Отправить уведомление");
            Console.WriteLine("6. Генерация отчета");
            Console.WriteLine("7. Создать пользователя");
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
                    AssignExecutor();
                    break;
                case 3:
                    TrackProgress();
                    break;
                case 4:
                    AddComment();
                    break;
                case 5:
                    SendNotification();
                    break;
                case 6:
                    GenerateReport();
                    break;
                case 7:
                    CreateUser();
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

            Task newTask = new Task(taskName, taskDeadline, taskPriority);
            Tasks.Add(newTask);
            Console.WriteLine("Задача успешно создана!");
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
            Users.Add(newUser);
            Console.WriteLine("Пользователь успешно создан!");
        }

        // Назначение исполнителя задачи
        static void AssignExecutor()
        {
            if (Tasks.Count == 0)
            {
                Console.WriteLine("Нет активных задач");
                return;
            }
            
            Task currentTask = GetUserInputTask();
            User currentUser = GetUserInputUser();
            
            Tasks[currentTask.Id - 1].Executor = currentUser;
            Console.WriteLine($"Ответственный {currentUser.Name} успешно назначен на задачу {currentTask.Name}");
        }
        
        /// <summary>
        /// Получение ИД задачи от пользователя
        /// </summary>
        /// <returns>int</returns>
        static Task GetUserInputTask()
        {
            Console.WriteLine("Укажите ИД задачи: ");
            
            // Выводим список задач для удобного ввода данных.
            foreach (Task task in Tasks)
            {
                Console.WriteLine($"Наименование: {task.Name}, ИД: {task.Id}");
            }
            
            if (int.TryParse(Console.ReadLine(), out int taskId))
            {
                // Проверяем что ИД задачи входит в список.
                if (Tasks.Count >= taskId && taskId > 0)
                {
                    taskId -= 1; // вычитаем 1 чтобы получить позицию в листе.
                    return Tasks[taskId];
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

        static User GetUserInputUser()
        {
            Console.WriteLine("Укажите ИД пользователя: ");
            
            // Выводим список пользователей для удобного ввода данных.
            foreach (User user in Users)
            {
                Console.WriteLine($"Имя: {user.Name}, ИД: {user.Id}");
            }
            
            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                foreach (User user in Users)
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

        // Отслеживаем прогресс задач
        static void TrackProgress()
        {
            Console.WriteLine("Отслеживание прогресса надо реализовывать");
        }

        // Добавляем комментарии к задаче
        static void AddComment()
        {
            Console.Write("Выберите номер задачи для комментирования: ");
            int taskIndex = int.Parse(Console.ReadLine()) - 1;

            if (taskIndex >= 0 && taskIndex < Tasks.Count)
            {
                Console.Write("Ваш комментарий: ");
                string comment = Console.ReadLine();
                Tasks[taskIndex].Comment = comment;
                Console.WriteLine("Комментарий добавлен успешно!");
            }
            else
            {
                Console.WriteLine("Указанная задача не найдена.");
            }
        }

        // Отправка уведомлений исполнителям
        static void SendNotification()
        {
            Console.WriteLine("Отправку уведомлений надо реализовывать");
        }

        // Генерация отчета по задачам
        static void GenerateReport()
        {
            Console.WriteLine("Отчет надо реализовывать");
        }

        // Завершение программы
        static void ExitApp()
        {
            Environment.Exit(0);
        }
    }
}

