using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    public class Task
    {
        // Поле для отслеживания ID
        private static int NextId = 1;

        public static void SetNextId(int NewId)
        {
            NextId = NewId;
        }

        /// <summary>
        /// ИД задачи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название задачи
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание задачи
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Сроки выполнения задачи
        /// </summary>
        public DateTime Deadlines { get; set; }

        /// <summary>
        /// Приоритет задачи
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// Комментарий к задаче
        /// </summary>
        public string Comment { get; set; }// Как вариант сделать коммент отдельным классом, с указанием автора, заголовком, 

        /// <summary>
        /// Исполнитель задачи
        /// </summary>
        public User Executor { get; set; }

        /// <summary>
        /// Прогресс(статус) задачи.
        /// </summary>
        public TaskProgress Progress { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="Description">Описание</param>
        /// <param name="Deadlines">Сроки</param>
        /// <param name="Priority">Приоритет</param>
        /// <param name="Executor">Исполнитель</param>
        /// <param name="Comment">Комментарии</param>
        public Task(string Name, DateTime Deadlines, string Priority, string Description = "")
        {
            this.Id = NextId++;
            this.Name = Name;
            this.Description = Description;
            this.Deadlines = Deadlines;
            this.Priority = Priority;
            this.Executor = User.GetDefaultUser();
            this.Comment = "";
            this.Progress = TaskProgress.New;
        }
    }
}
