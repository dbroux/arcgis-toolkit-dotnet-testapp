// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#elif SILVERLIGHT
using System;
using System.Windows;
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
        public TemplatePickerSample()
        {
            InitializeComponent();
#if !WINDOWS_PHONE
            MyTOCControl.Message += (s, message) => LogMessage(message);
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

#if WINDOWS_PHONE
        private void TestMemoryLeak(object sender, EventArgs e)
#else
        private void TestMemoryLeak(object sender, RoutedEventArgs e)
#endif
        {
            // Create a template picker that should be released immediatly since no more referenced
            var templatePicker = new Controls.TemplatePicker {Layers = MyTemplatePicker.Layers};
            ObjectTracker.Track(templatePicker);
            LogMessage(ObjectTracker.GarbageCollect());
        }


#if WINDOWS_PHONE
        private void GarbageCollect(object sender, EventArgs e)
#else
        private void GarbageCollect(object sender, RoutedEventArgs e)
#endif
        {
            LogMessage(ObjectTracker.GarbageCollect());
        }

    }
}
