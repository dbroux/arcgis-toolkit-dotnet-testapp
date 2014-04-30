// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Layers;
#if NETFX_CORE
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#else
using System.Windows.Controls;
#endif

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Internal
{
    static class Utils
    {
        public static string GetLayerName(Layer layer)
        {
            string name = layer.DisplayName;
            if (string.IsNullOrEmpty(name))
            {
                var featureLayer = layer as FeatureLayer;
                if (featureLayer != null)
                {
                    name = featureLayer.FeatureTable.Name;
                    if (string.IsNullOrEmpty(name) && featureLayer.FeatureTable is GeodatabaseFeatureServiceTable) // not initialized yet
                        name = ((GeodatabaseFeatureServiceTable)featureLayer.FeatureTable).ServiceUri;
                }
                if (string.IsNullOrEmpty(name))
                    name = layer.GetType().Name; // fallback value
            }

            return name;
        }


        public static void LogMessage(TextBox statusTextBox, string message)
        {
            statusTextBox.Text += "\n" + message;
#if SILVERLIGHT
            // Find ScrollViewer parent
            System.Windows.DependencyObject obj = statusTextBox;
            while(!(obj is ScrollViewer) && obj != null)
            {
                obj = System.Windows.Media.VisualTreeHelper.GetParent(obj);
            }
            var scrollViewer = obj as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.UpdateLayout();
                scrollViewer.ScrollToVerticalOffset(double.MaxValue);
            }
#elif NETFX_CORE
            var grid = (Grid)VisualTreeHelper.GetChild(statusTextBox, 0);
            for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
            {
                var scrollViewer = VisualTreeHelper.GetChild(grid, i) as ScrollViewer;
                if (scrollViewer != null)
                {
                    scrollViewer.ChangeView(null, scrollViewer.ExtentHeight, null);
                    break;
                }
            }
#else
            statusTextBox.ScrollToEnd();
#endif
        }

        internal static void LogMapViewEvents(MapView mapView, Action<string> logMessage)
        {
            mapView.LayerLoaded += (s, e) =>
            {
                string errorMessage = null;
                if (e.LoadError != null)
                {
                    errorMessage = " Error: " + e.LoadError.Message;
                    if (e.LoadError.InnerException != null)
                        errorMessage += Environment.NewLine + "  -->" + e.LoadError.InnerException.Message;
                }
                string message = "LayerLoaded: " + GetLayerName(e.Layer) + errorMessage;
                logMessage(message);
            };
            mapView.LayerUnloaded += (s, e) =>
            {
                string message = "LayerUnloaded: " + GetLayerName(e.Layer);
                logMessage(message);
            };
            mapView.SpatialReferenceChanged += (sender, args) =>
            {
                const string message = "SpatialReference Changed" ;
                logMessage(message);
            };
        }

        public static void TrackMap(Map map)
        {
            ObjectTracker.Track(map);
            TrackLayers(map.Layers);
        }

        private static void TrackLayers(IEnumerable<Layer> layers)
        {
            if (layers != null)
            {
                ObjectTracker.Track(layers);
                foreach (var layer in layers)
                {
                    ObjectTracker.Track(layer);
                    var groupLayer = layer as GroupLayer;
                    if (groupLayer != null)
                        TrackLayers(groupLayer.ChildLayers);
                }
            }
        }
    }
}
