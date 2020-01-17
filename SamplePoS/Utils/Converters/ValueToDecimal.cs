
using System;
using Windows.UI.Xaml.Data;

namespace SamplePoS.Utils.Converters
{
    public class ValueToDecimal: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value != null)
            {
                return value.ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if(value != null)
            {
                decimal.TryParse(value.ToString(), out decimal decimalValue);
                return decimalValue;
            }
            return decimal.Zero;
        }
    }
}
