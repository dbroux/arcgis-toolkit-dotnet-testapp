using Windows.UI.Xaml.Controls.Primitives;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Esri.ArcGISRuntime.Toolkit.TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = SampleDatasource.Current;
        }

        private void SampleListOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = sender as Selector;
            if (selector == null || !(selector.SelectedItem is Sample))
                return;

            ObjectTracker.GarbageCollect();
            var pagetype = (selector.SelectedItem as Sample).Page;
            Frame.Navigate(pagetype);
            selector.SelectedItem = null;
        }
    }
}
