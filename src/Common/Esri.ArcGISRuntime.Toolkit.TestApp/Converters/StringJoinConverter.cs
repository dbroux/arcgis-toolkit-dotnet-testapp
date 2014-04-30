// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.using System;

using System;
using System.Collections.Generic;
using System.Linq;
#if NETFX_CORE
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
#endif
#pragma warning disable 1573 // Missing parameter in the XML comment

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Converters
{
    /// <summary>
    /// Used by Attribution custom styles to join attribution text
    /// </summary>
    public sealed class StringJoinConverter : IValueConverter
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter
#if NETFX_CORE
            , string language)
#else
            , System.Globalization.CultureInfo culture)
#endif
        {
            if (value is IEnumerable<string>)
            {
                return string.Join(parameter is string ? (string)parameter : "", (value as IEnumerable<string>).ToArray());
            }
            return value;
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <returns>
        /// The value to be passed to the source object.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter
#if NETFX_CORE
            , string language)
#else
            , System.Globalization.CultureInfo culture)
#endif
        {
            throw new NotImplementedException();
        }
    }
}
