using LibraryProject.Transversal.Common;
using Microsoft.Extensions.Logging;


namespace LibraryProject.Transversal.Logging
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogError(Exception exction, string message, params object[] args)
        {
            _logger.LogError(message, args);
        }
    }
}
