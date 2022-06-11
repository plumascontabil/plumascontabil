using Demonstrativo.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net.NetworkInformation;

namespace Demonstrativo.ExtentionLogger
{
    public class AppLogger
    {

        public void Log<State>(
            LogLevel logLevel,
            EventId eventId,
            State state,
            Exception exception,
            Func<State,Exception,string> formato
            )
        {
            if (formato == null)
            {
                throw new ArgumentNullException(nameof(formato));
            }
            var mensagem = formato(state, exception);
            if (string.IsNullOrEmpty(mensagem))
            {
                return;
            }
            if (exception != null)
            {
                mensagem = exception.ToString();
            }

            var eventLog = new LogEvent()
            {
                Message = mensagem,
                EventId = eventId.Id,
                LogLevel = logLevel.ToString(),
                CreatedAt = DateTime.UtcNow
            };



        }

        private bool IsEnabled()
        {
            throw new NotImplementedException();
        }
    }
}
