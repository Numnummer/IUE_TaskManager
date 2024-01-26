using NtTaskWebServer.Framework.Attributes;
using NtTaskWebServer.Framework.Helpers;
using NtTaskWebServer.Model;
using NtTaskWebServer.Model.Task;
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
            var view = new View("View/Dashboard.htm", "text/html");
            await WebHelper.SendViewAsync(context, view);
        }

        [NeedAuth(Role.Reader)]
        public async Task GetTasksAsync(HttpListenerContext context)
        {
            var tasks = await TaskHelper.UpdateAllTasksAsync(context);
            await WebHelper.SendTasksAsync(context, tasks);
        }

        [NeedAuth(Role.Reader)]
        public async Task GetTaskHtmlAsync(HttpListenerContext context)
        {
            var view = new View("View/TaskCard.htm", "text/html");
            await WebHelper.SendViewAsync(context, view);
        }
        public async Task PostExitFromAccountAsync(HttpListenerContext context)
        {
            await WebHelper.DeleteSessionAsync(context);
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
            await WebHelper.SendOkAsync(context, "ok");
        }

        [NeedAuth(Role.Owner)]
        public async Task PostRemoveTaskAsync(HttpListenerContext context)
        {
            using var requestStream = context.Request.InputStream;
            var id = await JsonSerializer.DeserializeAsync<Guid>(requestStream);
            var success = await TaskHelper.RemoveTaskAsync(context, id);
            if (success)
            {
                await WebHelper.SendOkAsync(context, "removed");
            }
            else
            {
                await WebHelper.Send400Async(context, "нельзя удалить");
            }
        }

        [NeedAuth(Role.Owner)]
        public async Task PostDecreaseTaskStatusAsync(HttpListenerContext context)
        {
            using var requestStream = context.Request.InputStream;
            var id = await JsonSerializer.DeserializeAsync<Guid>(requestStream);
            if (await TaskHelper.DecreaseTaskStatusAsync(context, id))
            {
                await WebHelper.SendOkAsync(context, "ok");
            }
            await WebHelper.Send400Async(context, "not decreased");
        }

        [NeedAuth(Role.Owner)]
        public async Task PostIncreaseTaskStatusAsync(HttpListenerContext context)
        {
            using var requestStream = context.Request.InputStream;
            var id = await JsonSerializer.DeserializeAsync<Guid>(requestStream);
            if (await TaskHelper.IncreaseTaskStatusAsync(context, id))
            {
                await WebHelper.SendOkAsync(context, "ok");
            }
            await WebHelper.Send400Async(context, "not increased");
        }

        [NeedAuth(Role.Owner)]
        public async Task PostFriendDashboardAsync(HttpListenerContext context)
        {
            using var requestStream = context.Request.InputStream;
            var friend = await JsonSerializer.DeserializeAsync<string>(requestStream);
            var userName = SessionHelper.GetUserName(context);
            if (friend==null || userName==null)
            {
                await WebHelper.Send400Async(context, "Hе валидное имя друга или пользователя");
                return;
            }
            if (!await DatabaseHelper.HasFriendAsync(userName, friend))
            {
                await WebHelper.Send400Async(context, $"{userName} и {friend} не друзья");
                return;
            }
            await WebHelper.DeleteSessionAsync(context);
            var cookie = await SessionHelper.MakeSessionCookieAsync(friend, Role.Reader);
            await WebHelper.SendJsonObjectAsync(context, cookie);
        }
    }
}

