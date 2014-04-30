using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
#if NETFX_CORE
using Windows.UI.Xaml.Controls;
#else
using System.Windows.Controls;
#endif

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
    /// <summary>
    /// Scale Line Test Sample.
    /// </summary>
    public sealed partial class ScaleLineSample : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleLineSample"/> class.
        /// </summary>
        public ScaleLineSample()
        {
            InitializeComponent();
            ObjectTracker.Track(this);
            ObjectTracker.Track(MyScaleLine);
            ObjectTracker.Track(MyMapView);
            Utils.TrackMap(MyMap);
#if WINDOWS_PHONE_APP
            var navigationHelper = new NavigationHelper(this);
#endif
        }
    }
}
