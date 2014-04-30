using System;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
#else
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
#endif
#pragma warning disable 1573 // Missing parameter in the XML comment

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Converters
{
    /// <summary>
    /// Used by ScaleLine custom style for localizing units
    /// </summary>
#if NETFX_CORE
    [ContentProperty(Name="ResourceDictionary")]
#else
    [ContentProperty("ResourceDictionary")]
#endif
    public sealed class ObjectToObjectConverter : IValueConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectToObjectConverter"/> class.
        /// </summary>
        public ObjectToObjectConverter()
        {
            ResourceDictionary = new ResourceDictionary();
        }

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
#if NETFX_CORE
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string key = value.ToString();
            return ResourceDictionary.ContainsKey(key) ? ResourceDictionary[key] : key;
        }
#else
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string key = value.ToString();
            return ResourceDictionary.Contains(key) ? ResourceDictionary[key] : key;
        }
#endif

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

        /// <summary>
        /// The resource dictionary used for the conversion.
        /// <remarks>
        /// This is the content property of the class.
        /// </remarks>
        /// </summary>
        /// <value>The resource dictionary.</value>
        public ResourceDictionary ResourceDictionary { get; set; }
    }
}
