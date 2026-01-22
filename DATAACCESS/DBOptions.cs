using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATAACCESS
{
    public class DBOptions
    {
        public List<DBOptionItems> connections { get; set; } = [];
        public string name { get; set; } = string.Empty;
    }
    public class DBOptionItems
    {
        public string name { get; set; } = string.Empty;
        public string server { get; set; } = string.Empty;
        public string dataBase { get; set; } = string.Empty;
        public string user { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;

    }
}
