using Microsoft.Extensions.Logging;
using System;

namespace Demonstrativo.ExtentionLogger
{
    public class AppLoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly string _connectionString;


        public AppLoggerProvider(Func<string, LogLevel, bool> filter, string connectionString)
        {
            _filter = filter;
            _connectionString = connectionString;
        }

        //public ILogger CreateLogger(string nomeCategoria)
        //{
            //return new AppLogger(nomeCategoria, _filter, _connectionString);
        //}

        public void Dispose()
        {

        }
    }
}
