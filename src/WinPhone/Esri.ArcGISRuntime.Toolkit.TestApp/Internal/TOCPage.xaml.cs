using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Internal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TocPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TocPage"/> class.
        /// </summary>
        public TocPage()
        {
            InitializeComponent();
            TocControl = MyTocControl;
            var navigationHelper = new NavigationHelper(this);
        }

        internal TocControl TocControl { get; private set; }

        private void AddLayer_OnClick(object sender, RoutedEventArgs e)
        {
            MyTocControl.AddLayer_OnClick(sender, e);
        }

        private void SwitchLayers_OnClick(object sender, RoutedEventArgs e)
        {
            MyTocControl.SwitchLayers_OnClick(sender, e);
        }


        private void ClearLayers_OnClick(object sender, RoutedEventArgs e)
        {
            MyTocControl.ClearLayers_OnClick(sender, e);
        }

        private void AddStreetMapLayer_OnClick(object sender, RoutedEventArgs e)
        {
            MyTocControl.AddStreetMapLayer_OnClick(sender, e);
        }

        private void AddGroupLayerWithFL_OnClick(object sender, RoutedEventArgs e)
        {
            MyTocControl.AddGroupLayerWithFL_OnClick(sender, e);
        }

        private void AddGroupLayer_OnClick(object sender, RoutedEventArgs e)
        {
            MyTocControl.AddGroupLayer_OnClick(sender, e);
        }
    }
}
