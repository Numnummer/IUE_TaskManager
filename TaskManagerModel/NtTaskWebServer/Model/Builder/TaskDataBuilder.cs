using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Model.Builder
{
    public static class TaskDataBuilder
    {
        public static async Task<TaskManagerModel.Task[]> BuildTaskDataByDataReaderAsync(NpgsqlDataReader reader)
        {
            var taskData = new List<TaskManagerModel.Task>();
            while (await reader.ReadAsync())
            {
                var id = reader.GetValue(0) as Guid? ?? throw new Exception("Invalid id");
                var name = reader.GetValue(1) as string ?? throw new Exception("Invalid name");
                var start_time = reader.GetValue(2) as DateTime? ?? throw new Exception("Invalid start_time");
                var deadline = reader.GetValue(3) as DateTime? ?? throw new Exception("Invalid deadline");
                var priority = reader.GetValue(4) as int? ?? throw new Exception("Invalid piority");

                try
                {
                    var status = reader.GetValue(5) as string ?? throw new Exception("Invalid status");
                    taskData.Add(new TaskManagerModel.Task()
                    {
                        Id = id,
                        Name = name,
                        StartTime = start_time,
                        Deadline = deadline,
                        Priority = (uint)priority,
                        Status = GetStatus(status)
                    });
                }
                catch (Exception e)
                {

                    throw;
                }

            }

            return taskData.ToArray();
        }
        public static TaskManagerModel.TaskStatus GetStatus(string status)
        {
            return status switch
            {
                "NotStarted" => TaskManagerModel.TaskStatus.NotStarted,
                "InProcess" => TaskManagerModel.TaskStatus.InProcess,
                "Done" => TaskManagerModel.TaskStatus.Done,
                "Expired" => TaskManagerModel.TaskStatus.Expired
            };
        }
    }
}
