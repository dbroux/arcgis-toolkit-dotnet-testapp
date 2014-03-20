using System.Windows.Controls;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
	/// <summary>
	/// Interaction logic for LegendSample.xaml
	/// </summary>
	public partial class LegendSample : Page
	{
		public LegendSample()
		{
			InitializeComponent();
			ObjectTracker.Track(this);
		}

		~LegendSample()
		{
			
		}
	}
}
