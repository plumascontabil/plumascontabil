

using Demonstrativo.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Demonstrativo.ExtentionLogger
{
    public class RepositorioLogger
    {
        private string ConnectionString { get; set; }

        public RepositorioLogger(string connection)
        {
            ConnectionString = connection;
        }

        private bool ExecuteNonQuery(string commandStr, List<SqlParameter> paramList)
        {
            var result = false;
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                using (var command = new SqlCommand(commandStr, conn))
                {
                    command.Parameters.AddRange(paramList.ToArray());
                    var count = command.ExecuteNonQuery();
                    result = count > 0;
                }
            }
            return result;
        }

        public bool InsertLog(LogEvento log)
        {
            var command = $@"INSERT INTO [58_demonstrativo].[58_admin].[EventLog] ([EventId],[LogLevel],[Message],[CreatedTime]) VALUES (@EventId, @LogLevel, @Message, @CreatedTime)";
            var paramList = new List<SqlParameter>
            {
                new SqlParameter("EventId", log.EventId),
                new SqlParameter("LogLevel", log.LogLevel),
                new SqlParameter("Message", log.Message),
                new SqlParameter("CreatedTime", log.CreatedTime)
            };

            return ExecuteNonQuery(command, paramList);
        }
    }
}
