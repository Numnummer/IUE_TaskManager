using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Model
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
                var start_time = reader.GetValue(1) as DateTimeOffset? ?? throw new Exception("Invalid start_time");
                var deadline = reader.GetValue(1) as DateTimeOffset? ?? throw new Exception("Invalid deadline");
                var priority = reader.GetValue(1) as uint? ?? throw new Exception("Invalid piority");
                taskData.Add(new TaskManagerModel.Task()
                {
                    Id = id,
                    Name = name,
                    StartTime=start_time,
                    Deadline=deadline,
                    Priority=priority
                });
            }

            return taskData.ToArray();
        }
    }
}
