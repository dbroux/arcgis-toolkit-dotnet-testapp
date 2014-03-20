using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
using System.Windows;

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
            MyAttribution.Layers = new LayerCollection();
            ObjectTracker.Track(MyAttribution.Layers);
            LogMessage("Attribution.Layers initialized with a new collection not displayed in the map");
            MyMapView.Visibility = Visibility.Collapsed;
            MyMap.Layers = null;
        }

        private void TestMemoryLeak(object sender, RoutedEventArgs e)
        {
            // Create a control that should be released immediatly since no more referenced (but not working due to Layer.PropertyChanged handler)
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
