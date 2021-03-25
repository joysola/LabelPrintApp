using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LabelPrint.App.Converters
{
    /// <summary>
    /// 显示 单打、双打
    /// </summary>
    public class GridSampleInfoVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var printModel = (int?)value;
            var tag = System.Convert.ToInt32(parameter);
            if (tag == 1 && printModel == 3)
            {
                return Visibility.Visible;
            }
            else if (tag == printModel)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
