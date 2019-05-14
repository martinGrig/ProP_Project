using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace EventManager.Converters
{
    [ValueConversion(typeof(Visitor), typeof(BitmapImage))]
    class RfidScanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the full path
           
            
            // By default, we presume an image
            var image = "Images/ScanOrBuyTicket.png";

            // If the path is null, ignore
            if (value != null)
            {
                Visitor visitor = (Visitor)value;
                if(visitor.RfidCode == null)
                {
                    image = "Images/Scan Braclet.png";
                }
                else
                {
                    image = "Images/RFID account successful.png";
                }
                
            }
            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
