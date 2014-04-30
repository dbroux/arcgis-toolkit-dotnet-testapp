// (c) Copyright ESRI.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Layers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Controls;
#endif

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Internal
{
    /// <summary>
    /// Interaction logic for TocControl.xaml
    /// </summary>
    public partial class TocControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TocControl"/> class.
        /// </summary>
        public TocControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Layers to display in the TOC
        /// </summary>
        public IEnumerable<Layer> Layers
        {
            get { return (IEnumerable<Layer>)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        /// <summary>
        /// Identifies the Layers property.
        /// </summary>
        public static readonly DependencyProperty LayersProperty =
            DependencyProperty.Register("Layers", typeof(IEnumerable<Layer>), typeof(TocControl), new PropertyMetadata(null, OnLayersPropertyChanged));

        private LayerCollection _layers; 
        private static void OnLayersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TocControl)d)._layers = e.NewValue as LayerCollection;
        }

        /// <summary>
        /// Occurs when a message shouold be displayed
        /// </summary>
        public event EventHandler<string> Message;

        private void LogMessage(string message)
        {
            if (Message != null)
                Message(this, message);
        }

        internal void RemoveLayer_OnClick(object sender, RoutedEventArgs e)
        {
            var layer = ((FrameworkElement) sender).DataContext as Layer;
            var gl = FindParent(layer);
            if (gl == null)
                _layers.Remove(layer);
            else
                gl.ChildLayers.Remove(layer);
            LogMessage("Removed Layer " + Utils.GetLayerName(layer));
        }

        static int _index;
        internal async void AddLayer_OnClick(object sender, RoutedEventArgs e)
        {
            var featureLayer = await CreateFeatureLayer();
            var gl = GetCurrentParent();
            if (gl == null)
                _layers.Add(featureLayer);
            else
                gl.ChildLayers.Add(featureLayer);
        }

        private async Task<FeatureLayer> CreateFeatureLayer()
        {
            string url = "http://sampleserver6.arcgisonline.com/arcgis/rest/services/Military/FeatureServer/" + (_index + 2);
            _index = ++_index % 5;
            var ft = new GeodatabaseFeatureServiceTable(new Uri(url)) { UseAdvancedSymbology = true };
            var featureLayer = new FeatureLayer(ft);
            await featureLayer.InitializeAsync();
            featureLayer.DisplayName = featureLayer.FeatureTable.ServiceInfo.Name;
            LogMessage("Added Layer " + Utils.GetLayerName(featureLayer));
            //featureLayer.MinScale = MyMapView.Scale * 4;
            //featureLayer.MaxScale = MyMapView.Scale / 4;
            ObjectTracker.Track(featureLayer);
            return featureLayer;

        }

        internal void AddStreetMapLayer_OnClick(object sender, RoutedEventArgs e)
        {
            var layer = new OpenStreetMapLayer { Opacity = 0.5, DisplayName = "OpenStreetMapLayer" };
            var gl = GetCurrentParent();
            if (gl == null)
                _layers.Add(layer);
            else
                gl.ChildLayers.Add(layer);
            LogMessage("Added Layer " + Utils.GetLayerName(layer));
            ObjectTracker.Track(layer);
        }

        static int _groupLayerIndex;
        internal void AddGroupLayer_OnClick(object sender, RoutedEventArgs e)
        {
            var layer = new GroupLayer { DisplayName = "GroupLayer#" + ++_groupLayerIndex };
            var gl = GetCurrentParent();
            if (gl == null)
                _layers.Add(layer);
            else
                gl.ChildLayers.Add(layer);
            LogMessage("Added Layer " + Utils.GetLayerName(layer));
            ObjectTracker.Track(layer);
        }

        internal async void AddGroupLayerWithFL_OnClick(object sender, RoutedEventArgs e)
        {
            var layer = new GroupLayer { DisplayName = "GroupLayer#" + ++_groupLayerIndex };
            layer.ChildLayers.Add(await CreateFeatureLayer());
            var gl = GetCurrentParent();
            if (gl == null)
                _layers.Add(layer);
            else
                gl.ChildLayers.Add(layer);
            LogMessage("Added Layer " + Utils.GetLayerName(layer));
            ObjectTracker.Track(layer);
        }

        internal void ClearLayers_OnClick(object sender, RoutedEventArgs e)
        {
            var gl = GetCurrentParent();
            if (gl == null)
                _layers.Clear();
            else
                gl.ChildLayers.Clear();
            LogMessage("Layers cleared");
        }

        internal void SwitchLayers_OnClick(object sender, RoutedEventArgs e)
        {
            var gl = GetCurrentParent();
            ObservableCollection<Layer> coll = gl == null ? Layers as ObservableCollection<Layer> : gl.ChildLayers;
            if (coll != null && coll.Count >= 2)
            {
#if SILVERLIGHT
                var layer = coll.First();
                coll.RemoveAt(0);
                coll.Insert(1, layer);
#else
                coll.Move(0, 1);
#endif
                LogMessage("First layer moved after second");
            }
        }

        private GroupLayer FindParent(Layer layer)
        { 
            return layer == null ? null : _layers.EnumerateAllLayers().OfType<GroupLayer>().FirstOrDefault(gl => gl.ChildLayers != null && gl.ChildLayers.Contains(layer));
        }

        private GroupLayer GetCurrentParent()
        {
            var layer = MySelector.SelectedItem as Layer;
            var parent = layer as GroupLayer;
            return parent ?? FindParent(layer);
        }

    }
}
