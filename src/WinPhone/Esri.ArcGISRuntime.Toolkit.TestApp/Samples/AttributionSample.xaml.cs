using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using System.Collections.ObjectModel;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
	public partial class AttributionSample : PhoneApplicationPage
	{
		public AttributionSample()
		{
			InitializeComponent();
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

		private void TestMemoryLeak(object sender, EventArgs e)
		{
			// Create a control that should be released immediatly since no more referenced
			var control = new Controls.Attribution { Layers = MyAttribution.Layers };
			ObjectTracker.Track(control);
		}

		private void GarbageCollect(object sender, EventArgs e)
		{
			LogMessage(ObjectTracker.GarbageCollect());
		}


		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			var tocPage = e.Content as TOCPage;
			if (tocPage != null)
			{
				tocPage.DataContext = MyAttribution;
				tocPage.MyTOCControl.Message += (s, message) => LogMessage(message);
			}
			base.OnNavigatedFrom(e);
		}

		private void GoToTOCPage(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Internal/TOCPage.xaml", UriKind.Relative));
		}
	}
}