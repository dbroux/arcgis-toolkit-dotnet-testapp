// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

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
        public AttributionSample()
        {
            InitializeComponent();
			MyTOCControl.Message += (s, message) => LogMessage(message);
			Utils.LogMapViewEvents(MyMapView, LogMessage);
			ObjectTracker.Track(this);
            ObjectTracker.Track(MyAttribution);
            ObjectTracker.Track(MyMapView);
			Utils.TrackMap(MyMap);
            MyMap.InitialExtent = new Envelope(-15000000, 0, -5000000, 10000000, SpatialReferences.WebMercator);
        }

		private void LogMessage(string message)
		{
			Utils.LogMessage(StatusText, message);
		}

		private void InitNewLayers_OnClick(object sender, RoutedEventArgs e)
        {
            MyAttribution.Layers = new ObservableCollection<Layer>();
            LogMessage("Attribution.Layers initialized with a new collection not displayed in the map");
            MyMapView.Visibility = Visibility.Collapsed;
        }

		private void TestMemoryLeak(object sender, RoutedEventArgs e)
		{
			// Create a control that should be released immediatly since no more referenced
			var control = new Controls.Attribution { Layers = MyAttribution.Layers };
			ObjectTracker.Track(control);
		}

		private void GarbageCollect(object sender, RoutedEventArgs e)
		{
			LogMessage(ObjectTracker.GarbageCollect());
		}
    }

}
