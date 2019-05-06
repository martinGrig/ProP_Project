using EventManager.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EventManager.Converters
{
    [ValueConversion(typeof(LogLine), typeof(bool))]
    class LogFileRegExConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LogLine line = (LogLine)value;
            Regex regex;
            switch (line.Type)
            {
                case "iban":
                    regex = new Regex(@"^[a-zA-Z]{2}[0-9]{2}[ ][a-zA-Z]{4}[ ][0-9]{4}[ ][0-9]{4}[ ][0-9]{2}");
                    if (!regex.IsMatch(line.Line))
                    {
                        return false;
                    }
                    break;
                case "user":
                    regex = new Regex(@"[0-9]*[ ][0-9]*[.][0-9]{2}");
                    if (!regex.IsMatch(line.Line))
                    {
                        return false;
                    }
                    break;


            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
