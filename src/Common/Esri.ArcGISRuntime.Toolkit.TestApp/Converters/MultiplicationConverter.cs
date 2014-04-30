using System;
using System.Globalization;
#if NETFX_CORE
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
#endif
#pragma warning disable 1573 // Missing parameter in the XML comment

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Converters
{
    /// <summary>
    /// Used by ScaleLine custom style for calculating sizes
    /// </summary>
    public sealed class MultiplicationConverter : IValueConverter
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
            if (value == null || (value is string && string.IsNullOrEmpty(value as string)))
                return value;
            double d = System.Convert.ToDouble(value);
            double frac = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            double ret = d * frac;
            if (targetType == typeof(double))
            {
                return ret;
            }
            if (targetType == typeof(float))
            {
                return (float)ret;
            }
            if (targetType == typeof(int))
            {
                return (int)Math.Round(ret);
            }
            if (targetType == typeof(string))
            {
                return ret.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            throw new NotSupportedException(string.Format("Conversion to {0} not supported", targetType));
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
            throw new NotSupportedException();
        }
    }
}
