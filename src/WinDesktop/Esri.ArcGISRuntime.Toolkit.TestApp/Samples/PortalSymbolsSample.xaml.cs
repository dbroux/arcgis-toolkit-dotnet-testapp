using Esri.ArcGISRuntime.Portal;
using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
	/// <summary>
	/// Interaction logic for PortalSymbolsSample.xaml
	/// </summary>
	public partial class PortalSymbolsSample : Page
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PortalSymbolsSample"/> class.
		/// </summary>
		public PortalSymbolsSample()
		{
			InitializeComponent();
			DataContext = new PortalSymbolsViewModel();
		}

		private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		private void SetClipboardOnClick(object sender, RoutedEventArgs e)
		{
			var code = ((FrameworkElement)sender).Tag.ToString();
			Clipboard.SetText(code);
			MessageBox.Show(code, ".Net code stored in the clipboard.");
		}
	}


	/// <summary>
	/// View Model for the PortalSymbols sample
	/// </summary>
	public class PortalSymbolsViewModel : INotifyPropertyChanged
	{
		private IEnumerable<ArcGISPortalItem> _allSymbolSets;
		// Backing fields for properties
		private IEnumerable<ArcGISPortalItem> _symbolSets;
		private ArcGISPortalItem _currentSymbolSet;
		private bool _preferByValue;
		private IEnumerable<SymbolViewModel> _symbolViewModels;
		private bool _isBusy;

		/// <summary>
		/// Initializes a new instance of the <see cref="PortalSymbolsViewModel"/> class.
		/// </summary>
		public PortalSymbolsViewModel()
		{
			var task = InitSymbolSetsAsync();
		}

		/// <summary>
		/// Gets the list of symbol sets (actually symbol category)
		/// </summary>
		public IEnumerable<ArcGISPortalItem> SymbolSets
		{
			get { return _symbolSets; }
			private set
			{
				if (_symbolSets != value)
				{
					_symbolSets = value;
					RaisePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the current symbol set.
		/// </summary>
		/// <value>
		/// The current symbol set.
		/// </value>
		public ArcGISPortalItem CurrentSymbolSet
		{
			get { return _currentSymbolSet; }
			set
			{
				if (_currentSymbolSet != value)
				{
					_currentSymbolSet = value;
					var task = InitSymbolsAsync();
					RaisePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether [prefer by value].
		/// </summary>
		/// <value>
		///   <c>true</c> if [prefer by value]; otherwise, <c>false</c>.
		/// </value>
		public bool PreferByValue
		{
			get { return _preferByValue; }
			set
			{
				if (_preferByValue != value)
				{
					_preferByValue = value;
					var currentSymbolSet = CurrentSymbolSet; // keep current symbol set to be able to restore it as current
					SetSymbolSets();

					// Restore current symbol set (or use first one)
					if (currentSymbolSet != null)
						currentSymbolSet = SymbolSets.FirstOrDefault(i => i.Title == currentSymbolSet.Title); // find out the current symbolset based on title (the list has changed)
					CurrentSymbolSet = currentSymbolSet ?? SymbolSets.FirstOrDefault();

					RaisePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets the symbol view models.
		/// </summary>
		/// <value>
		/// The symbol view models.
		/// </value>
		public IEnumerable<SymbolViewModel> SymbolViewModels
		{
			get { return _symbolViewModels; }
			private set
			{
				if (_symbolViewModels != value)
				{
					_symbolViewModels = value;
					RaisePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is busy.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
		/// </value>
		public bool IsBusy
		{
			get { return _isBusy; }
			private set
			{
				_isBusy = value;
				RaisePropertyChanged();
			}
		}


		// Get symbol sets portal items 
		private async Task InitSymbolSetsAsync()
		{
			IsBusy = true;
			try
			{
				ArcGISPortal portal = await ArcGISPortal.CreateAsync(); // www.arcgis.com by default
				string groupQuery = portal.ArcGISPortalInfo.SymbolSetsGroupQuery;
				SearchResultInfo<ArcGISPortalGroup> groups = await portal.SearchGroupsAsync(new SearchParameters(groupQuery));
				ArcGISPortalGroup group = groups.Results.First(); // Group that drives the symbol sets for the current culture
				SearchResultInfo<ArcGISPortalItem> items = await portal.SearchItemsAsync(new SearchParameters("group:" + group.Id) { Limit = 100 }); // one portal item by symbolset
				_allSymbolSets = items.Results.ToArray();
				SetSymbolSets();
				CurrentSymbolSet = SymbolSets.FirstOrDefault();
			}
			catch (Exception e)
			{
				MessageBox.Show("Error while getting symbol sets: " + e.Message);
			}
		}

		// Some item may be duplicated one 'by value', one with the ImageUrl only. Select one depending on PreferByValue flag
		private void SetSymbolSets()
		{
			if (_allSymbolSets != null)
				SymbolSets = _allSymbolSets.OrderBy(item => item.Title + (item.TypeKeywords.Contains("by value") ^ PreferByValue))
										   .GroupBy(i => i.Title)
										   .Select(g => g.First());
		}

		// Create a symbol view model from a symbol json dictionary
		private SymbolViewModel CreateFromDictionary(IDictionary<string, object> dict)
		{
			var json = new JavaScriptSerializer().Serialize(dict); // convert back the json dictionary to a string
			Symbol symbol = Symbol.FromJson(json); // create the symbol from a string

			var url = dict.ContainsKey("url") ? dict["url"].ToString() : null;
			var title = dict.ContainsKey("title") ? dict["title"].ToString() : null;
			if (string.IsNullOrEmpty(title) && dict.ContainsKey("name"))
				title = dict["name"].ToString();
			return new SymbolViewModel { Title = title, Json = json, ImageUrl = url, Symbol = symbol };
		}

		// Init the symbols from the current symbolset
		private async Task InitSymbolsAsync()
		{
			SymbolViewModels = null;
			if (CurrentSymbolSet != null)
			{
				IsBusy = true;
				try
				{
					using (Stream stream = await CurrentSymbolSet.GetItemDataAsync())
					{
						if (stream != null)
						{
							string json;
							using (var streamReader = new StreamReader(stream))
								json = streamReader.ReadToEnd();

							var jss = new JavaScriptSerializer();
							var symbDicts = jss.Deserialize<IEnumerable<IDictionary<string, object>>>(json);
							SymbolViewModels = symbDicts.Select(CreateFromDictionary).ToArray();
						}
					}
				}
				catch (Exception e)
				{
					MessageBox.Show("Error while getting symbols: " + e.Message);
				}
				IsBusy = false;
			}
		}


		#region INotifyPropertyChanged
		/// <summary>
		///  Occurs when a non-dependency property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion INotifyPropertyChanged
	}

	/// <summary>
	/// ViewModel for a symbol
	/// </summary>
	public class SymbolViewModel
	{
		/// <summary>
		/// Gets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		public string Title { get; internal set; }

		/// <summary>
		/// Gets the image URL.
		/// </summary>
		public string ImageUrl { get; internal set; }

		/// <summary>
		/// Gets the json.
		/// </summary>
		public string Json { get; internal set; }

		/// <summary>
		/// Gets the symbol.
		/// </summary>
		public Symbol Symbol { get; internal set; }

		/// <summary>
		/// Gets the .Net code.
		/// </summary>
		public string Code
		{
			get
			{
				var escapedJson = System.Text.RegularExpressions.Regex.Replace(Json, "\"[^\"]*\":", Environment.NewLine + "    $&"); // insert newline before each dictionary key
				escapedJson = System.Text.RegularExpressions.Regex.Replace(escapedJson, "}$", Environment.NewLine + "$&") // insert new line at end of json
									.Replace("\"", "\"\""); // escape the quotes
				return string.Format("var symbol = Symbol.FromJson(@\"{0}\");", escapedJson);
			}
		}
	}
}
