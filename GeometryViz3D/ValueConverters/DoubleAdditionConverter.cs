using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GeometryViz3D.ValueConverters
{
    public class DoubleAdditionConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double v = (double)value;
            double p = Double.Parse((string)parameter);
            return v + p;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double v = (double)value;
            double p = Double.Parse((string)parameter);
            return v - p;
        }

        #endregion
    }
}
