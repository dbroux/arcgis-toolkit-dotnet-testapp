using System.Windows;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;

namespace Esri.ArcGISRuntime.Toolkit.TestApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnLoadCompleted(System.Windows.Navigation.NavigationEventArgs e)
		{
			Resources["ShouldTrackVisibility"] = ObjectTracker.ShouldTrack() ? Visibility.Visible : Visibility.Collapsed;
 			base.OnLoadCompleted(e);
		}
	}
}
