using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace EventManager.Converters
{
    class EmailColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string emailaddress = (string)value;
                if (emailaddress != "")
                {
                    try
                    {
                        MailAddress m = new MailAddress(emailaddress);

                        return Brushes.Green;
                    }
                    catch (FormatException)
                    {
                        return Brushes.Red;
                    }
                }

            }
            return Brushes.Red;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
