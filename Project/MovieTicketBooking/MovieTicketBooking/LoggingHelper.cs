using NLog;
using System;

namespace MovieTicketBooking
{
    public static class LoggingHelper
    {
        // Create an NLog logger instance
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // Log Information
        public static void LogInfo(string message)
        {
            logger.Info(message);
        }

        // Log Warnings
        public static void LogWarning(string message)
        {
            logger.Warn(message);
        }

        // Log Errors
        public static void LogError(string message, Exception ex = null)
        {
            logger.Error(ex, message);
        }

        // Log Debug messages
        public static void LogDebug(string message)
        {
            logger.Debug(message);
        }
    }
}
