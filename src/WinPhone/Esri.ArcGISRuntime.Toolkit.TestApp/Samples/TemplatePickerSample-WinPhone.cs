// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Toolkit.Controls;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
    /// <summary>
    /// Interaction logic for TemplatePickerSample.xaml
    /// </summary>
    public partial class TemplatePickerSample
    {
        /// <summary>
        /// Invoked immediately after the Page is unloaded and is no longer the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the navigation that has unloaded the current Page.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var tocPage = e.Content as TocPage;
            if (tocPage != null)
            {
                tocPage.DataContext = MyTemplatePicker;
                tocPage.TocControl.Message += (s, message) => LogMessage(message);
            }
            if (e.NavigationMode == NavigationMode.Back)
            {
                // Reset cache so sample can restart from scratch
                var cacheSize = Frame.CacheSize;
                Frame.CacheSize = 0;
                Frame.CacheSize = cacheSize;
            }
            base.OnNavigatedFrom(e);
        }

        private async void InitializeFeatureServicesOnLoaded(object sender, RoutedEventArgs e)
        {
            // Initialize 
            var templatePicker = sender as TemplatePicker;
            if (templatePicker != null)
            {
                foreach (var flayer in templatePicker.Layers.OfType<FeatureLayer>().ToArray())
                {
                    try
                    {
                        await flayer.InitializeAsync();
                    }
                    catch { }
                }
            }
            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void GoToTocPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (TocPage));
        }
    }
}
