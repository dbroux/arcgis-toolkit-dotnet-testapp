// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
#if NETFX_CORE
using Windows.UI.Xaml;
#else
using System.Windows;
#endif

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
    /// <summary>
    /// Interaction logic for TemplatePickerSample.xaml
    /// </summary>
    public partial class TemplatePickerSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplatePickerSample"/> class.
        /// </summary>
        public TemplatePickerSample()
        {
            InitializeComponent();
#if WINDOWS_PHONE_APP
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled; // cache needed when coming back from TOC
            var navigationHelper = new NavigationHelper(this);
#else
            MyTocControl.Message += (s, message) => LogMessage(message);
#endif
            Utils.LogMapViewEvents(MyMapView, LogMessage);
            ObjectTracker.Track(this);
            ObjectTracker.Track(MyTemplatePicker);
            ObjectTracker.Track(MyMapView);
            Utils.TrackMap(MyMap);
            MyMap.InitialExtent = new Envelope(-15000000, 0, -5000000, 10000000, SpatialReferences.WebMercator);
        }

        private void TemplatePicker_OnTemplatePicked(object sender, Controls.TemplatePicker.TemplatePickedEventArgs e)
        {
            string name = Utils.GetLayerName(e.Layer);
            string message = "Event TemplatePicked --> Type='" + (e.FeatureType == null ? "null" : e.FeatureType.Name) + "'  Template='" + e.FeatureTemplate.Name + "'  Layer='" + name + "'";
            LogMessage(message);
        }

        private void LogMessage(string message)
        {
            Utils.LogMessage(StatusText, message);
        }

        private void InitNewLayers_OnClick(object sender, RoutedEventArgs e)
        {
            MyTemplatePicker.Layers = new LayerCollection();
            LogMessage("TemplatePicker.Layers initialized with a new collection not displayed in the map");
            MyMapView.Visibility = Visibility.Collapsed;
            MyTemplatePicker.Scale = double.NaN;
            ObjectTracker.Track(MyTemplatePicker.Layers);
        }

        private void TestMemoryLeak(object sender, RoutedEventArgs e)
        {
            // Create a template picker that should be released immediatly since no more referenced
            var templatePicker = new Controls.TemplatePicker {Layers = MyTemplatePicker.Layers};
            ObjectTracker.Track(templatePicker);
            LogMessage(ObjectTracker.GarbageCollect());
        }

        private void GarbageCollect(object sender, RoutedEventArgs e)
        {
            LogMessage(ObjectTracker.GarbageCollect());
        }

    }
}
