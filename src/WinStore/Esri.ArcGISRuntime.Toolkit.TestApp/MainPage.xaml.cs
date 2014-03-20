using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Esri.ArcGISRuntime.Toolkit.TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = SampleDatasource.Current;
        }


        private void SampleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObjectTracker.GarbageCollect();
            var pagetype = ((sender as ListBox).SelectedItem as Sample).Page;
            MainFrame.Navigate(pagetype);
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ObjectTracker.GarbageCollect();
        }
    }
}
