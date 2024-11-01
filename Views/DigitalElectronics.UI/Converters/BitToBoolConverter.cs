using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.UI.Converters
{

    /// <summary>
    /// Converts a <see cref="Nullable{Boolean}"/> to non-nullable <see cref="Boolean"/>,
    /// where `null` is converted to `false`
    /// </summary>
    [ValueConversion(typeof(Bit), typeof(bool))]
    public class BitToBoolConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Bit bit && (bool)bit;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Bit(value.Equals(true));
        }
    }
}
