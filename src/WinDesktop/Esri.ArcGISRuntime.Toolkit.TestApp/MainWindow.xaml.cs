using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Esri.ArcGISRuntime.Toolkit.TestApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = SampleDatasource.Current;
		}


		private void SampleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var pagetype = ((Sample) ((ListBox) sender).SelectedItem).Page;
			string url = "Samples/" + pagetype.Name + ".xaml";
			MainFrame.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
			ObjectTracker.GarbageCollect();
		}
	}
}
