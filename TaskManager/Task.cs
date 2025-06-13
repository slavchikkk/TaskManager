using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    public class Task
    {
        //Поле для отслеживания ID
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
        public string TaskName { get; set; }

        /// <summary>
        /// Описание задачи
        /// </summary>
        public string TaskDescription { get; set; }

        /// <summary>
        /// Сроки выполнения задачи
        /// </summary>
        public DateTime TaskDeadlines { get; set; }

        /// <summary>
        /// Приоритет задачи
        /// </summary>
        public string TaskPriority { get; set; }

        /// <summary>
        /// Комментарий к задаче
        /// </summary>
        public string TaskComment { get; set; }//Как вариант сделать коммент отдельным классом, с указанием автора, заголовком, 

        /// <summary>
        /// Исполнитель задачи
        /// </summary>
        public User TaskExecutor { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="TaskName">Имя</param>
        /// <param name="TaskDescription">Описание</param>
        /// <param name="TaskDeadlines">Сроки</param>
        /// <param name="TaskPriority">Приоритет</param>
        /// <param name="TaskExecutor">Исполнитель</param>
        /// <param name="TaskComment">Комментарии</param>
        public Task(string TaskName, DateTime TaskDeadlines, string TaskPriority, string TaskDescription = "")
        {
            this.Id = NextId++;
            this.TaskName = TaskName;
            this.TaskDescription = TaskDescription;
            this.TaskDeadlines = TaskDeadlines;
            this.TaskPriority = TaskPriority;
            this.TaskExecutor = User.GetDefaultUser();
            this.TaskComment = "";
        }
    }
}
