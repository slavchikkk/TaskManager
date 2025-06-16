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
            Console.Clear();
            Console.WriteLine("\nСписок действий:");
            Console.WriteLine("1. Создать задачу");
            Console.WriteLine("2. Назначить ответственного");
            Console.WriteLine("3. Просмотреть прогресс");
            Console.WriteLine("4. Добавить комментарий");
            Console.WriteLine("5. Отправить уведомление");
            Console.WriteLine("6. Генерация отчета");
            Console.WriteLine("7. Выход");
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

        // Назначение исполнителя задачи
        static void AssignExecutor()
        {
            Console.WriteLine("Назначение исполнителя надо реализовывать");
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

