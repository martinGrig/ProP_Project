using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EventManager.Converters
{
    class LoanStandEnddateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                int days = (int)value;
                DateTime enddate = new DateTime(2019, 6, 6);
                int daysleft = (enddate.DayOfYear - DateTime.Now.DayOfYear);
                if(days < daysleft)
                {
                    return true;
                }
                else
                {
                    return false;
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
