using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    public class Task
    {
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
        public string Comment { get; set; }

        /// <summary>
        /// Исполнитель задачи
        /// </summary>
        public User Executor { get; set; }
        
        public List<User> Spectators { get; set; }

        /// <summary>
        /// Прогресс (статус) задачи
        /// </summary>
        public TaskProgress Progress { get; set; }

        /// <summary>
        /// Пустой конструктор для JSON-десериализации
        /// </summary>
        public Task()
        {
            Id = 0;
            Name = string.Empty;
            Description = string.Empty;
            Deadlines = DateTime.Now;
            Priority = "Normal";
            Comment = string.Empty;
            Executor = User.GetDefaultUser();
            Progress = TaskProgress.New;
            Spectators = new List<User>();
        }
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="Description">Описание</param>
        /// <param name="Deadlines">Сроки</param>
        /// <param name="Priority">Приоритет</param>
        /// <param name="Executor">Исполнитель</param>
        /// <param name="Comment">Комментарии</param>
        public Task(int id, string name, DateTime deadlines, string priority, string description = "", string comment = "", User executor = null, TaskProgress progress = TaskProgress.New)
        {
            Id = id;
            Name = name;
            Description = description;
            Deadlines = deadlines;
            Priority = priority;
            Comment = comment;
            Executor = executor ?? User.GetDefaultUser();
            Progress = progress;
        }
    }
}
