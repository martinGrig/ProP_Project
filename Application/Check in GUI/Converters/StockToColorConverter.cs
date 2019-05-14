using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace EventManager.Converters
{
    class StockToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                throw new InvalidOperationException("The targetType must be a Brush object");
            }
            if (values.Length < 2)
            {
                throw new ArgumentOutOfRangeException();
            }
            int stock = 0;
            int seenAmount = 0;
            int.TryParse(values[0].ToString(), out stock);
            int.TryParse(values[1].ToString(), out seenAmount);
            Brush bBrush = Brushes.Black;

            if (stock != seenAmount)
            {
                
                if (seenAmount >= 50)
                {
                    bBrush = Brushes.DarkOrange;
                }
                else if(seenAmount >= 25)
                {
                    bBrush = Brushes.Yellow;
                }
                else if(seenAmount >= 10)
                {
                    bBrush = Brushes.Orange;
                }
                else if(seenAmount >= 5)
                {
                    bBrush = Brushes.OrangeRed;
                }
                else
                {
                    bBrush = Brushes.Red;
                }
                
            }
            else if(stock == 0)
            {
                bBrush = Brushes.Red;
            }
            return bBrush;
        }
        

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
