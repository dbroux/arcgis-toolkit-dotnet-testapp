using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
using System.Windows;

namespace Esri.ArcGISRuntime.Toolkit.TestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.LoadCompleted" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Navigation.NavigationEventArgs" /> that contains the event data.</param>
        protected override void OnLoadCompleted(System.Windows.Navigation.NavigationEventArgs e)
        {
            Resources["ShouldTrackVisibility"] = ObjectTracker.ShouldTrack() ? Visibility.Visible : Visibility.Collapsed;
            base.OnLoadCompleted(e);
        }
    }
}
