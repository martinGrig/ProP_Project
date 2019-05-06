using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Objects
{
    public class LogLine
    {
        public string Line { get; private set; }
        public string Type { get; private set; }

        public LogLine(string line, string type)
        {
            Line = line;
            Type = type;
        }
    }
}
