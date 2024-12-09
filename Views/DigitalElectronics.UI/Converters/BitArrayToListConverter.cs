using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.UI.Converters;

[ValueConversion(typeof(BitArray), typeof(IList<bool>))]
public class BitArrayToListConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is BitArray bitArray ? bitArray.ToArray() : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is IEnumerable<bool> bits ? new BitArray(bits) : value;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}
