using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Internal
{
	public partial class TOCPage : PhoneApplicationPage
	{
		public TOCPage()
		{
			InitializeComponent();
		}

		private void AddLayer_OnClick(object sender, EventArgs e)
		{
			MyTOCControl.AddLayer_OnClick(sender, new RoutedEventArgs());
		}

		private void SwitchLayers_OnClick(object sender, EventArgs e)
		{
			MyTOCControl.SwitchLayers_OnClick(sender, new RoutedEventArgs());
		}


		private void ClearLayers_OnClick(object sender, EventArgs e)
		{
			MyTOCControl.ClearLayers_OnClick(sender, new RoutedEventArgs());
		}

		private void AddStreetMapLayer_OnClick(object sender, EventArgs e)
		{
			MyTOCControl.AddStreetMapLayer_OnClick(sender, new RoutedEventArgs());
		}

		private void AddGroupLayerWithFL_OnClick(object sender, EventArgs e)
		{
			MyTOCControl.AddGroupLayerWithFL_OnClick(sender, new RoutedEventArgs());
		}

		private void AddGroupLayer_OnClick(object sender, EventArgs e)
		{
			MyTOCControl.AddGroupLayer_OnClick(sender, new RoutedEventArgs());
		}
		
	}
}