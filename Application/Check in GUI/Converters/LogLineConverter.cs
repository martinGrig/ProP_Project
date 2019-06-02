using EventManager.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EventManager.Converters
{
    class LogLineConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
            {
                try
                {
                    int count = (int)values[0];
                    string par = (string)parameter;
                    if (par == "a" && count > 0)
                    {
                        return true;
                    }
                    else if (par == "s" && count > 0)
                    {
                        return true;
                    }
                    else if (par == "u" && count > 0)
                    {

                        List<LogLine> allLines = (List<LogLine>)values[1];
                        if (count < allLines.Where(x => x.IsEnabled).Count())
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    return false;
                }

            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
