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
    class AddEmployeeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Job job;
            string firstName;
            string lastName;
            Shop shop;
            LoanStand loanStand;
            try
            {
                job = (Job)values[0];
                firstName = (string)values[1];
                lastName = (string)values[2];
            }
            catch
            {
                return false;
            }
            if (job == null)
            {
                return false;
            }
            if (job.Id.Contains("s"))
            {

                shop = (Shop)values[3];
                if (shop == null)
                {
                    return false;
                }
            }
            if (job.Id.Contains("l"))
            {

                loanStand = (LoanStand)values[4];
                if (loanStand == null)
                {
                    return false;
                }
            }
            if (firstName != null && firstName.Trim() != "" && lastName != null && lastName.Trim() != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
