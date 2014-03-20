// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Layers;
using System;
using System.Windows;
using Esri.ArcGISRuntime.Toolkit.Controls;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
    /// <summary>
    /// Interaction logic for TemplatePickerSample.xaml
    /// </summary>
    public partial class TemplatePickerSample
    {
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var tocPage = e.Content as TOCPage;
            if (tocPage != null)
            {
                tocPage.DataContext = MyTemplatePicker;
                tocPage.MyTOCControl.Message += (s, message) => LogMessage(message);
            }
            base.OnNavigatedFrom(e);
        }

        private async void InitializeFeatureServicesOnLoaded(object sender, RoutedEventArgs e)
        {
            // Initialize 
            var templatePicker = sender as TemplatePicker;
            foreach (var flayer in templatePicker.Layers.OfType<FeatureLayer>().ToArray())
            {
                try
                {
                    await flayer.InitializeAsync();
                }
                catch{}
            }
            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void GoToTOCPage(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Internal/TOCPage.xaml", UriKind.Relative));
        }

    }
}
