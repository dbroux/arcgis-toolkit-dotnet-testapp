// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
    /// <summary>
    /// Interaction logic for LegendSample.xaml
    /// </summary>
    public partial class LegendSample
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
                tocPage.DataContext = MyLegend;
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

        private void GoToTocPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (TocPage));
        }

    }
}
