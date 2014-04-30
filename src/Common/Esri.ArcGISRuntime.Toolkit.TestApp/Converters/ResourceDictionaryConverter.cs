// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.using System;

using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Converters
{
	/// <summary>
	///  Workaround to WinStore KeyValuePair bug: http://social.msdn.microsoft.com/Forums/windowsapps/en-US/234a17ad-975f-42f6-aa91-7212deda4190/targetexception-error-in-binding?forum=winappswithcsharp
	/// </summary>
	public class ResourceDictionaryConverter : IValueConverter
	{
		/// <summary>
		/// Modifies the source data before passing it to the target for display in the UI.
		/// </summary>
		/// <param name="value">The source data being passed to the target.</param>
		/// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="language"></param>
		/// <returns>
		/// The value to be passed to the target dependency property.
		/// </returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			// Additionally set 'Default' as first item
			return ((ResourceDictionary)value).OrderBy(kvp => "Default" != kvp.Key as string).Select(kvp => new StringValuePair(kvp.Key as string, kvp.Value)).ToList();
		}

		/// <summary>
		/// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
		/// </summary>
		/// <param name="value">The target data being passed to the source.</param>
		/// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="language"></param>
		/// <returns>
		/// The value to be passed to the source object.
		/// </returns>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}

	internal class StringValuePair
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
