using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Objects
{
    public class LogLine : ObservableObject
    {
        public string Line { get; private set; }
        public string Type { get; private set; }
        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        public LogLine(string line, string type)
        {
            Line = line;
            Type = type;
        }
    }
}
