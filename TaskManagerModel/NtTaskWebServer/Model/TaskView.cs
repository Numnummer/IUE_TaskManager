using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Model
{
    public class TaskView : View
    {
        public string TaskHtml { get; private set; }
        public string PriorityDto { get; private set; }
        public string StatusDto { get; private set; }
        public TaskView(string fileName, string contentType,
            TaskManagerModel.Task task) : base(fileName, contentType)
        {
            if (task==null)
            {
                throw new ArgumentNullException($"{nameof(task)} is null");
            }
            var document = new HtmlWeb().Load(fileName) ??
                throw new Exception("Не найден файл представления задачи");
            var name = document.GetElementbyId("name") ??
                throw new Exception("Не найден id имени задачи");
            var startDate = document.GetElementbyId("startDate") ??
                throw new Exception("Не найден id даты старта задачи");
            var deadline = document.GetElementbyId("deadline") ??
                throw new Exception("Не найден id дедлайна задачи");
            name.InnerHtml=task.Name;
            startDate.InnerHtml=task.StartTime.ToString();
            deadline.InnerHtml=task.Deadline.ToString();

            TaskHtml=document.Text;
            PriorityDto=task.Priority.ToString();
            StatusDto=task.Status.ToString();
        }

    }
}
