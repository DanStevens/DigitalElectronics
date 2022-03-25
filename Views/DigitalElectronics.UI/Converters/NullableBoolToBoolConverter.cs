using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace DigitalElectronics.Demos.Converters
{

    /// <summary>
    /// Converts a <see cref="Nullable{Boolean}"/> to non-nullable <see cref="Boolean"/>,
    /// where `null` is converted to `false`
    /// </summary>
    [ValueConversion(typeof(bool?), typeof(bool))]
    public class NullableBoolToBoolConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && (bool)value;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
