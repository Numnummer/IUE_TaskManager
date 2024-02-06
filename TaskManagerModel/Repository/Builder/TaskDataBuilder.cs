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
        public static async Task<Models.Task[]> BuildTaskDataByDataReaderAsync(NpgsqlDataReader reader)
        {
            var taskData = new List<Models.Task>();
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
                    taskData.Add(new Models.Task()
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
        public static Models.TaskStatus GetStatus(string status)
        {
            return status switch
            {
                "NotStarted" => Models.TaskStatus.NotStarted,
                "InProcess" => Models.TaskStatus.InProcess,
                "Done" => Models.TaskStatus.Done,
                "Expired" => Models.TaskStatus.Expired
            };
        }
    }
}
