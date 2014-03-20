// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Data;
using System.Collections.Generic;
#else
using System.Windows;
#endif

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
    /// <summary>
    /// Interaction logic for LegendSample.xaml
    /// </summary>
    public partial class LegendSample
    {
        public LegendSample()
        {
            InitializeComponent();
#if !WINDOWS_PHONE
            MyTOCControl.Message += (s, message) => LogMessage(message);
#endif
            ObjectTracker.Track(this);
            ObjectTracker.Track(MyLegend);
            ObjectTracker.Track(MyMapView);
            Utils.TrackMap(MyMap);
            MyMap.InitialExtent = new Envelope(-15000000, 0, -5000000, 10000000, SpatialReferences.WebMercator);
        }

        private void OnLegendRefreshed(object sender, Controls.Legend.RefreshedEventArgs e)
        {
            string name = Utils.GetLayerName(e.LayerItem.Layer);
            string message = "Event Legend Refreshed. Layer= '" + name + "'" + (e.Error == null ? "" : e.Error.Message);
            LogMessage(message);
        }

        private void LogMessage(string message)
        {
            Utils.LogMessage(StatusText, message);
        }


        private void InitNewLayers_OnClick(object sender, RoutedEventArgs e)
        {
            MyLegend.Layers = new LayerCollection();
            LogMessage("Legend.Layers initialized with a new collection not displayed in the map");
            MyMapView.Visibility = Visibility.Collapsed;
            MyLegend.Scale = double.NaN;
            ObjectTracker.Track(MyLegend.Layers);
        }

#if WINDOWS_PHONE
        private void TestMemoryLeak(object sender, EventArgs e)
#else
        private void TestMemoryLeak(object sender, RoutedEventArgs e)
#endif
        {
            // Create a control that should be released immediatly since no more referenced
            var control = new Controls.Legend { Layers = MyLegend.Layers };
            ObjectTracker.Track(control);
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
