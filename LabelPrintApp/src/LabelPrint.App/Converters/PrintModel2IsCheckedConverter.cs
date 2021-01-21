using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LabelPrint.App.Converters
{
    public class PrintModel2IsCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var printModel = (int?)value;
            var tag = System.Convert.ToInt32(parameter);
            var isChecked = printModel == tag;
            return isChecked;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isChecked = (bool)value;
            var result = isChecked ? (int?)System.Convert.ToInt32(parameter) : null;
            return result;
        }
    }
}
