using Serilog;
using System.Diagnostics;


namespace LOGGER
{
    public interface ILoggerPersonalized
    {
        void Information(string format, params object[] objects);
        void Information(string message);

        void Fatal(string format, params object[] objects);
        void Fatal(string message);

        void Warning(string format, params object[] objects);
        void Warning(string message);

        void Error(string format, params object[] objects);
        void Error(string message);

        void Debug(string format, params object[] objects);
        void Debug(string message);

        void Verbose(string format, params object[] objects);
        void Verbose(string message);
    }
    public class Logger : ILoggerPersonalized
    {
        private static string? _pathLogFile;
        private static int? _fileSizeLimit;
        private static string? _structure;
        private static int? _limit;
        public const string header = "{0} DETALLE: {1}";
        public Logger(string pathLogFile, string level, int fileSizeLimit, string logStructure, int limit) { Create(pathLogFile, level, fileSizeLimit, logStructure, limit); }
        private static void Create(string pathLogFile, string level, int fileSizeLimit, string structure, int limit)
        {
            _pathLogFile = pathLogFile;
            _fileSizeLimit = fileSizeLimit;
            _structure = structure;
            _limit = limit;
            switch (level)
            {
                case "INFORMATION":
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.With(new ThreadIdEnricher())
                        .WriteTo.File(_pathLogFile, rollingInterval: RollingInterval.Day, outputTemplate: _structure, rollOnFileSizeLimit: true, fileSizeLimitBytes: _fileSizeLimit, retainedFileCountLimit: _limit)
                        .WriteTo.Console()
                        .MinimumLevel.Information()
                        .CreateLogger();
                    break;
                case "FATAL":
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.With(new ThreadIdEnricher())
                        .WriteTo.File(_pathLogFile, rollingInterval: RollingInterval.Day, outputTemplate: _structure, rollOnFileSizeLimit: true, fileSizeLimitBytes: _fileSizeLimit, retainedFileCountLimit: _limit)
                        .WriteTo.Console()
                        .MinimumLevel.Fatal()
                        .CreateLogger();
                    break;
                case "WARNING":
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.With(new ThreadIdEnricher())
                        .WriteTo.File(_pathLogFile, rollingInterval: RollingInterval.Day, outputTemplate: _structure, rollOnFileSizeLimit: true, fileSizeLimitBytes: _fileSizeLimit, retainedFileCountLimit: _limit)
                        .WriteTo.Console()
                        .MinimumLevel.Warning()
                        .CreateLogger();
                    break;
                case "ERROR":
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.With(new ThreadIdEnricher())
                        .WriteTo.File(_pathLogFile, rollingInterval: RollingInterval.Day, outputTemplate: _structure, rollOnFileSizeLimit: true, fileSizeLimitBytes: _fileSizeLimit, retainedFileCountLimit: _limit)
                        .WriteTo.Console()
                        .MinimumLevel.Error()
                        .CreateLogger();
                    break;
                case "DEBUG":
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.With(new ThreadIdEnricher())
                        .WriteTo.File(_pathLogFile, rollingInterval: RollingInterval.Day, outputTemplate: _structure, rollOnFileSizeLimit: true, fileSizeLimitBytes: _fileSizeLimit, retainedFileCountLimit: _limit)
                        .WriteTo.Console()
                        .MinimumLevel.Debug()
                        .CreateLogger();
                    break;
                default:
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.With(new ThreadIdEnricher())
                        .WriteTo.File(_pathLogFile, rollingInterval: RollingInterval.Day, outputTemplate: _structure, rollOnFileSizeLimit: true, fileSizeLimitBytes: _fileSizeLimit, retainedFileCountLimit: _limit)
                        .WriteTo.Console()
                        .MinimumLevel.Verbose()
                        .CreateLogger();
                    break;
            }
        }
        private static string GetStackTraceInfo()
        {
            var stackFrame = new StackTrace().GetFrame(2);
            string methodName = stackFrame.GetMethod().Name;
            string className = stackFrame.GetMethod().ReflectedType.FullName;
            return string.Format("Class: \"{0}\" Method: \"{1}\"", className, methodName);
        }

        public void Information(string format, params object[] objects)
        {
            string location = GetStackTraceInfo();
            string message = string.Format(format, objects);
            string _header = header;
            Log.Information(string.Format(_header, location, message));
        }
        public void Information(string message)
        {
            string location = GetStackTraceInfo();
            string _header = header;
            Log.Information(string.Format(_header, location, message));
        }

        public void Fatal(string format, params object[] objects)
        {
            string message = string.Format(format, objects);
            string location = GetStackTraceInfo();
            string _header = header;
            Log.Fatal(string.Format(_header, location, message));
        }
        public void Fatal(string message)
        {
            string location = GetStackTraceInfo();
            string _header = header;
            Log.Fatal(string.Format(_header, location, message));
        }

        public void Warning(string format, params object[] objects)
        {
            string message = string.Format(format, objects);
            string location = GetStackTraceInfo();
            string _header = header;
            Log.Warning(string.Format(_header, location, message));
        }
        public void Warning(string message)
        {
            string location = GetStackTraceInfo();
            string _header = header;
            Log.Warning(string.Format(_header, location, message));
        }

        public void Error(string format, params object[] objects)
        {
            string message = string.Format(format, objects);
            string location = GetStackTraceInfo();
            string _header = header;
            Log.Error(string.Format(_header, location, message));
        }
        public void Error(string message)
        {
            string location = GetStackTraceInfo();
            string _header = header;
            Log.Error(string.Format(_header, location, message));
        }

        public void Debug(string format, params object[] objects)
        {
            string location = GetStackTraceInfo();
            string message = string.Format(format, objects);
            string _header = header;
            Log.Debug(string.Format(_header, location, message));
        }
        public void Debug(string message)
        {
            string location = GetStackTraceInfo();
            string _header = header;
            Log.Debug(string.Format(_header, location, message));
        }

        public void Verbose(string format, params object[] objects)
        {
            string message = string.Format(format, objects);
            string location = GetStackTraceInfo();
            string _header = header;
            Log.Verbose(string.Format(_header, location, message));
        }
        public void Verbose(string message)
        {
            string location = GetStackTraceInfo();
            string _header = header;
            Log.Verbose(string.Format(_header, location, message));
        }
    }
}
