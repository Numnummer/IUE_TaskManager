﻿using MyWebFramework;
using NtTaskWebServer.Framework.Attributes;
using NtTaskWebServer.Framework.Helpers;
using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NtTaskWebServer.Controller
{
    public class DashboardController
    {
        [NeedAuth(Role.Reader)]
        public async Task GetDashboardAsync(HttpListenerContext context)
        {
            await TaskHelper.UpdateAllTasksAsync(context);
            var view = new View("View/Dashboard.htm", "text/html");
            await WebHelper.SendViewAsync(context, view);
        }
        public async Task PostExitFromAccountAsync(HttpListenerContext context)
        {
            WebHelper.DeleteSession(context);
            await WebHelper.SendOkAsync(context, "ok");
        }

        [NeedAuth(Role.Owner)]
        public async Task PostCreateTaskAsync(HttpListenerContext context)
        {
            using var requestStream = context.Request.InputStream;
            try
            {
                var task = await JsonSerializer.DeserializeAsync<TaskData>(requestStream);
                if (!ValidationHelper.IsValidTaskData(task))
                {
                    throw new Exception();
                }
                var priority = uint.TryParse(task.Priority, out var priorityNumber);
                await TaskHelper.CreateTaskAsync(context, task.Name, task.Deadline, priorityNumber);
            }
            catch
            {
                await WebHelper.Send400Async(context, "Не правильные данные");
                return;
            }

            var view = new View("View/TaskCard.htm", "text/html");
            await WebHelper.SendViewAsync(context, view);
        }
    }
}
