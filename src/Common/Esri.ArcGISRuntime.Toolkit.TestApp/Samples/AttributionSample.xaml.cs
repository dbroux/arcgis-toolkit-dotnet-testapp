// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using System.Collections.ObjectModel;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
#if NETFX_CORE
using Windows.UI.Xaml;
#else
using System.Windows;
#endif

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
    /// <summary>
    /// Interaction logic for AttributionSample.xaml
    /// </summary>
    public partial class AttributionSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributionSample"/> class.
        /// </summary>
        public AttributionSample()
        {
            InitializeComponent();
            Utils.LogMapViewEvents(MyMapView, LogMessage);
            ObjectTracker.Track(this);
            ObjectTracker.Track(MyAttribution);
            ObjectTracker.Track(MyMapView);
            Utils.TrackMap(MyMap);
            MyMap.InitialViewpoint = new ViewpointExtent(new Envelope(-15000000, 0, -5000000, 10000000, SpatialReferences.WebMercator));

#if WINDOWS_PHONE_APP
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled; // cache needed when coming back from TOC
            var navigationHelper = new NavigationHelper(this);
#else
            MyTocControl.Message += (s, message) => LogMessage(message);
#endif
        }

        private void LogMessage(string message)
        {
            Utils.LogMessage(StatusText, message);
        }

        private void InitNewLayers_OnClick(object sender, RoutedEventArgs e)
        {
            MyAttribution.Layers = new ObservableCollection<Layer>();
            ObjectTracker.Track(MyAttribution.Layers);
            LogMessage("Attribution.Layers initialized with a new collection not displayed in the map");
            MyMapView.Visibility = Visibility.Collapsed;
            MyMap.Layers = null;
        }

        private void TestMemoryLeak(object sender, RoutedEventArgs e)
        {
            // Create a control that should be released immediately since no more referenced
            var control = new Controls.Attribution { Layers = MyAttribution.Layers };
            ObjectTracker.Track(control);
            LogMessage(ObjectTracker.GarbageCollect());
        }

        private void GarbageCollect(object sender, RoutedEventArgs e)
        {
            LogMessage(ObjectTracker.GarbageCollect());
        }
    }

}
