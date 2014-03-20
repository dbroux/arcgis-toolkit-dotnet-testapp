// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.using System;

using System;
using System.Linq;
#if NETFX_CORE
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
#else
using System.Windows;
#endif

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Converters
{
    // Workaround to WinStore KeyValuePair bug: http://social.msdn.microsoft.com/Forums/windowsapps/en-US/234a17ad-975f-42f6-aa91-7212deda4190/targetexception-error-in-binding?forum=winappswithcsharp
    public class ResourceDictionaryConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Additionaly set 'Default' as first item
            return ((ResourceDictionary)value).OrderBy(kvp => "Default" != kvp.Key as string).Select(kvp => new StringValuePair(kvp.Key as string, kvp.Value)).ToList();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class StringValuePair
    {
        public StringValuePair(string key, object value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
