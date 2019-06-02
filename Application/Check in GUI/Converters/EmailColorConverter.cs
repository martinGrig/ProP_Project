using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace EventManager.Converters
{
    class EmailColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            Regex emailRegex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                + "@"
                + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");

            if ((string)parameter == "tb")
            {
                if (value != null)
                {
                    string emailaddress = (string)value;

                    if (emailRegex.Match(emailaddress).ToString() == emailaddress && emailaddress != "")
                    {
                        return Brushes.Green;
                    }
                    return Brushes.Red;
                }
            }
            else if ((string)parameter == "b")
            {
                if (value != null)
                {
                    string emailaddress = (string)value;
                    if (emailRegex.Match(emailaddress).ToString() == emailaddress && emailaddress != "")
                    {
                        return true;
                    }
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

