// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Converters
{
    public sealed class StringJoinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable<string>)
            {
                return string.Join(parameter is string ? (string)parameter : "", (value as IEnumerable<string>).ToArray());
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
