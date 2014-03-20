// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
using System;
using System.Windows;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
    /// <summary>
    /// Interaction logic for FreeTestSample.xaml
    /// </summary>
    public partial class FreeTestSample
    {
        public FreeTestSample()
        {
            InitializeComponent();
            MyTOCControl.Message += (s, message) => LogMessage(message);
            Utils.LogMapViewEvents(MyMapView, LogMessage);
            ObjectTracker.Track(this);
            ObjectTracker.Track(MyMapView);
            foreach (var layer in MyMap.Layers)
            {
                ObjectTracker.Track(layer);
                var groupLayer = layer as GroupLayer;
                if (groupLayer != null && groupLayer.ChildLayers != null)
                {
                    foreach (var sublayer in groupLayer.ChildLayers)
                    {
                        ObjectTracker.Track(sublayer);
                    }
                }
            }
            MyMap.InitialExtent = new Envelope(-15000000, 0, -5000000, 10000000, SpatialReferences.WebMercator);
        }

        private void LogMessage(string message)
        {
            Utils.LogMessage(StatusText, message);
        }


        private void InitNewLayers_OnClick(object sender, RoutedEventArgs e)
        {
            MyMap.Layers = new LayerCollection();
            LogMessage("MyMap.Layers initialized with a new collection not displayed in the map");
            ObjectTracker.Track(MyMap.Layers);
        }
        private void OnClickFreeTest(object sender, EventArgs e)
        {
            var renderer = new UniqueValueRenderer();
            var g = new Graphic();
            var symbol = renderer.GetSymbol(g);
            MessageBox.Show(symbol == null ? "null" : symbol.ToJson());
        }

        private void GarbageCollect(object sender, EventArgs e)
        {
            LogMessage(ObjectTracker.GarbageCollect());
        }

    }
}

