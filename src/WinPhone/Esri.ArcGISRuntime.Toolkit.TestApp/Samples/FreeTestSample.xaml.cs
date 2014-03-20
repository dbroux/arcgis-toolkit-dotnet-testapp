using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Esri.ArcGISRuntime.Geometry;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
	public partial class FreeTestSample : PhoneApplicationPage
	{
		public FreeTestSample()
		{
			InitializeComponent();
			MyMap.InitialExtent = new Envelope(-15000000, 0, -5000000, 10000000, SpatialReferences.WebMercator);
		}
	}
}