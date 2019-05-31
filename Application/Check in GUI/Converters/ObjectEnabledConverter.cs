using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EventManager.Converters
{
    class ObjectEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if((string)parameter == "b")
                {
                    int index = (int)value;
                    if(index == -1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                if((string)parameter == "cb")
                {
                    int index = (int)value;
                    if (index == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
