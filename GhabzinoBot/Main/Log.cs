using NLog;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ayantech.WebService
{
    public static class Log
    {
        private static Logger _logger;

        static Log()
        {
            try
            {
                //  LogManager.Configuration = new XmlLoggingConfiguration(ProjectValues.LogConfigFilePath, true);
                _logger = LogManager.GetLogger("main");
            }
            catch
            {
                return;
            }
        }
        public static void Test(string message, double executionTime = 0, string userName = null, [CallerFilePath] string filePath = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            _logger.Fatal(Format(message, executionTime, userName, filePath, memberName, lineNumber));
        }
        public static void Trace(string message, double executionTime, string userName = null, [CallerFilePath] string filePath = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {

            _logger.Trace(Format(message, executionTime, userName, filePath, memberName, lineNumber));
        }
        public static void Debug(string message, double executionTime, string userName = null, [CallerFilePath] string filePath = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            _logger.Debug(Format(message, executionTime, userName, filePath, memberName, lineNumber));
        }
        public static void Info(string message, double executionTime, string userName = null, [CallerFilePath] string filePath = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            _logger.Info(Format(message, executionTime, userName, filePath, memberName, lineNumber));
        }
        public static void Warn(string message, double executionTime, string userName = null, [CallerFilePath] string filePath = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            _logger.Warn(Format(message, executionTime, userName, filePath, memberName, lineNumber));
        }
        public static void Error(string message, double executionTime, string userName = null, [CallerFilePath] string filePath = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            _logger.Error(Format(message, executionTime, userName, filePath, memberName, lineNumber));
        }
        public static void Fatal(string message, double executionTime, string userName = null, [CallerFilePath] string filePath = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            _logger.Fatal(Format(message, executionTime, userName, filePath, memberName, lineNumber));
        }
        private static string Body(string message)
        {
            return $"[{message}]";
        }
        private static string Consumer(string userName)
        {
            if (string.IsNullOrEmpty(userName) == true)
                return "[]";

            var wsun = $"(UserName: {userName})";
            var iP = $"(IpAddress: {Helper.GetIpAddress()})";
            var userAgent = $"(UserAgent: {Helper.GetUserAgent()})";
            var calledUrl = $"(CalledUrl: {Helper.GetCalledUrl()})";

            return $"[{wsun}{iP}{userAgent}{calledUrl}]";
        }
        private static string CurrentMethod(string filePath, string memberName, int lineNumber)
        {
            return $"[{filePath}.{memberName} line:{lineNumber}";
        }
        private static string ExecutionTime(double executionTime)
        {
            return $"[{executionTime}]";
        }
        private static string Format(string message, double executionTime, string userName, string filePath, string memberName, int lineNumber)
        {
            filePath = filePath.Contains('\\') ? filePath.Substring(filePath.LastIndexOf('\\') + 1) : filePath;
            filePath = filePath.Contains('.') ? filePath.Substring(0, filePath.IndexOf('.')) : filePath;

            Monitoring(executionTime, filePath, memberName, lineNumber);
            var currentMethod = CurrentMethod(filePath, memberName, lineNumber);
            var executiontime = ExecutionTime(executionTime);
            var consumer = Consumer(userName);
            var body = Body(message);

            return $"{executiontime};{currentMethod};{consumer};{body}";
        }
        private static void Monitoring(double executionTime, string filePath, string memberName, int lineNumber)
        {
            try
            {
                if (ProjectValues.MonitoringMode && ProjectValues.MonitoringDictionary.Keys != null && ProjectValues.MonitoringDictionary.Keys.Count != 0)
                    foreach (var key in ProjectValues.MonitoringDictionary.Keys)
                        if (string.Equals(key.Split('-')[0], filePath, StringComparison.OrdinalIgnoreCase))
                        {
                            var value = ProjectValues.MonitoringDictionary[key];
                            if (value.Count < ProjectValues.MonitoringMaxSize)
                                value.AddLast(new MonitorValue($"{filePath}.{memberName} line:{lineNumber}", executionTime));
                        }
            }
            catch
            {
                return;
            }
        }
    }
}