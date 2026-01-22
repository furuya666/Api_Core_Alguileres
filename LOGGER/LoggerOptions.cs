using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGGER
{
    public class LoggerOptions
    {
        public string Path_Log_File { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public int Limit { get; set; }
        public int FileSizeLimitBytes { get; set; }
        public string Log_Structure { get; set; } = string.Empty;
    }
}
