using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.UI.Converters
{
    [ValueConversion(typeof(BitArray), typeof(string))]
    public class BitArrayToStringConverter : MarkupExtension, IValueConverter
    {
        public NumberFormat Format { get; set; } = NumberFormat.UnsignedHexadecimal;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            
            return (value is BitArray bitArray ? bitArray.ToString(Format) : value?.ToString()) ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
