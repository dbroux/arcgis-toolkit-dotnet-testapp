// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Portal;
using Esri.ArcGISRuntime.Security;
using Esri.ArcGISRuntime.Toolkit.Controls;
using Esri.ArcGISRuntime.Toolkit.Security;
using System;
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SignInChallengeHandlerSample : Page, INotifyPropertyChanged
	{
		//private const string PortalIWAUrl = "https://portaliwa.esri.com/gis/sharing/rest";
		private const string SecuredServiceUrl = "http://serverapps101.esri.com/arcgis/rest/services/USStatesUser1/MapServer";
		private const string FederatedFeatureServiceUrl = "http://services.arcgis.com/pmcEyn9tLWCoX7Dm/arcgis/rest/services/Test_feature_service_DBX/FeatureServer/0";

		/// <summary>
		/// Initializes a new instance of the <see cref="SignInChallengeHandlerSample"/> class.
		/// </summary>
		public SignInChallengeHandlerSample()
		{
			// Initialize challenge handler to allow storage in the credential locker and restore the credentials
			var im = IdentityManager.Current;
			// Remove existing credentials to simulate a start from scratch each time the sample is executed
			foreach (var crd in im.Credentials.ToArray())
				im.RemoveCredential(crd);

			im.ChallengeHandler = new SignInChallengeHandler { AllowSaveCredentials = true, CredentialSaveOption = CredentialSaveOption.Selected }; // set it to CredentialSaveOption.Hidden if it's not an user choice

			SignInOutCommand = new DelegateCommand(ToggleSignIn, p => !_isBusy && !string.IsNullOrEmpty(PortalUrl));

			InitializeComponent();
		}

		/// <summary>
		/// Gets or sets the portal URL.
		/// </summary>
		/// <value>
		/// The portal URL.
		/// </value>
		public string PortalUrl
		{
			get { return (string)GetValue(PortalUrlProperty); }
			set { SetValue(PortalUrlProperty, value); }
		}

		/// <summary>
		/// The portal URL property
		/// </summary>
		public static readonly DependencyProperty PortalUrlProperty = DependencyProperty.Register("PortalUrl", typeof(string), typeof(SignInChallengeHandlerSample), new PropertyMetadata(null, OnPortalUrlChanged));

		static void OnPortalUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var _ = ((SignInChallengeHandlerSample)d).OnPortalUrlChanged();
		}

		private async Task OnPortalUrlChanged()
		{
			IsBusy = true;
			await CreatePortalAsync(false); // try to instantiate a new portal object with the known credentials (without challenging for the native secured case)
			IsBusy = false;
		}

		private ArcGISPortal _arcGISPortal;
		/// <summary>
		/// Gets the ArcGISPortal (output parameter)
		/// </summary>
		/// <value>
		/// The ArcGISPortal.
		/// </value>
		public ArcGISPortal ArcGISPortal
		{
			get { return _arcGISPortal; }
			private set
			{
				if (_arcGISPortal != value)
				{
					_arcGISPortal = value;
					SignInButton.Content = IsSignedIn ? "Sign Out" : "Sign In";
					OnPropertyChanged("ArcGISPortal");
					OnPropertyChanged("IsAvailable");
					OnPropertyChanged("LoginName");
				}
			}
		}

		/// <summary>
		/// Gets the sign in out command.
		/// </summary>
		/// <value>
		/// The sign in out command.
		/// </value>
		public ICommand SignInOutCommand { get; private set; }

		private bool _isBusy;
		/// <summary>
		/// Gets or sets a value indicating whether this instance is busy.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
		/// </value>
		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				if (_isBusy != value)
				{
					_isBusy = value;
					OnPropertyChanged("IsBusy");
					((DelegateCommand)SignInOutCommand).OnCanExecuteChanged();
				}
			}
		}

		/// <summary>
		/// Gets the name of the login.
		/// </summary>
		/// <value>
		/// The name of the login.
		/// </value>
		public string LoginName
		{
			get { return IsAvailable ? (_arcGISPortal.CurrentUser == null ? "Anonymous" : _arcGISPortal.CurrentUser.FullName) : null; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is available.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is available; otherwise, <c>false</c>.
		/// </value>
		public bool IsAvailable
		{
			get { return _arcGISPortal != null && (_arcGISPortal.CurrentUser != null || _arcGISPortal.ArcGISPortalInfo.Access != PortalAccess.Private); }
		}

		private bool IsSignedIn
		{
			get { return ArcGISPortal != null && ArcGISPortal.CurrentUser != null; }
		}

		private async void ToggleSignIn(object parameter)
		{
			IsBusy = true;
			if (IsSignedIn) // Toggle to Signed out
			{
				await SignOut();
			}
			else // toggle to Signed In
			{
				await SignIn();
			}
			IsBusy = false;
		}

		private async Task SignOut()
		{
			// Remove all credentials (even those for external services, hosted services, federated services) from IM and from the CredentialLocker
			foreach (var crd in IdentityManager.Current.Credentials.ToArray())
				IdentityManager.Current.RemoveCredential(crd);
			var challengeHandler = IdentityManager.Current.ChallengeHandler as SignInChallengeHandler;
			if (challengeHandler != null)
				challengeHandler.ClearCredentialsCache(); // remove stored credentials

			// Create the portal without any credential (though we might still be logged if portal is secured with PKI/IWA (i.e actually not possible to logout))
			await CreatePortalAsync(false);
		}

		private async Task SignIn()
		{
			if (ArcGISPortal == null) // Portal secured with native. You were not able to instantiate the portal with the current credentials --> we need to instantiate the portal with challenge
			{
				await CreatePortalAsync(true);
				// After this step we might have a current user if the portal is secured with native --> test again !IsSignedIn
			}

			if (!IsSignedIn && ArcGISPortal != null) // Note: if the user canceled the previous native/PKI authentication, ArcGISPortal is null. In this case don't challenge again the user for a token
			{
				// We need a token credential to act as 'SignedIn'
				Credential crd = null;
				try
				{
					// Challenge for a credential.
					crd = await IdentityManager.Current.GetCredentialAsync(new CredentialRequestInfo { ServiceUri = ArcGISPortal.Uri.AbsoluteUri }, true);
				}
				catch { }
				if (crd != null)
				{
					// The credential has been added to IM. Create a new Portal with this credential.
					await CreatePortalAsync(true); // false should be OK as well. We are not supposed to challenge here.
				}
			}
		}

		// Instantiate a new portal with the current credentials
		private async Task CreatePortalAsync(bool withChallenge)
		{
			IsBusy = true;

			ArcGISPortal = null;
			Exception error = null;
			var challengeHandler = IdentityManager.Current.ChallengeHandler;
			if (!withChallenge)
			{
				// Deactivate the challenge handler temporarily before creating the portal (else challengehandler would be called for portal secured by native)
				IdentityManager.Current.ChallengeHandler = null;
			}

			ArcGISPortal portal = null;
			try
			{
				portal = await ArcGISPortal.CreateAsync(string.IsNullOrEmpty(PortalUrl) ? null : new Uri(PortalUrl));
			}
			catch (Exception e)
			{
				error = e;
			}

			if (!withChallenge)
				IdentityManager.Current.ChallengeHandler = challengeHandler; // Restore ChallengeHandler
			else if (error != null)
				MessageBox.Show("Error: " + error.Message);

			ArcGISPortal = portal;
		}

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}


		// Some tests
		private async void AccessSecuredServiceOnClick(object sender, RoutedEventArgs e)
		{
			var layer = new ArcGISDynamicMapServiceLayer { ServiceUri = SecuredServiceUrl };
			string message = "OK";
			try
			{
				await layer.InitializeAsync();
			}
			catch (Exception ex)
			{
				message = "Error: " + ex.Message;
			}
			MessageBox.Show(message);
		}
		private async void AccessFederatedFeatureServiceOnClick(object sender, RoutedEventArgs e)
		{
			var layer = new FeatureLayer(new Uri(FederatedFeatureServiceUrl));
			string message = "OK";
			try
			{
				await layer.InitializeAsync();
			}
			catch (Exception ex)
			{
				message = "Error: " + ex.Message;
			}
			MessageBox.Show(message);
		}

		internal async void TestPortalOnClick(object sender, RoutedEventArgs e)
		{
			if (ArcGISPortal == null)
				return;
			var portal = await ArcGISPortal.CreateAsync(new Uri(PortalUrl));
			if (portal == null)
				return;
			string message = portal.CurrentUser != null ? await GetUserInfo(portal.CurrentUser) : await GetPortalInfo(portal);
			MessageBox.Show(message);
		}

		private static async Task<string> GetUserInfo(ArcGISPortalUser user)
		{
			string info = "       LOGGED AS:" + Environment.NewLine;
			info += "FullName: " + user.FullName + Environment.NewLine;
			info += "Username: " + user.UserName + Environment.NewLine;
			info += "Description: " + user.Description + Environment.NewLine + Environment.NewLine;
			info += Environment.NewLine + "    Some User items:" + Environment.NewLine;
			foreach (var item in (await user.GetItemsAsync()).Take(12))
				info += item.Title + Environment.NewLine;
			return info;
		}


		private static async Task<string> GetPortalInfo(ArcGISPortal portal)
		{
			string info = "       ANONYMOUS LOGIN. PORTAL INFO:" + Environment.NewLine;
			info += "Portal name: " + portal.ArcGISPortalInfo.PortalName + Environment.NewLine;
			info += "Description: " + portal.ArcGISPortalInfo.Description + Environment.NewLine + Environment.NewLine;
			info += Environment.NewLine + "    Basemap Gallery:" + Environment.NewLine;
			foreach (var item in (await portal.ArcGISPortalInfo.SearchBasemapGalleryAsync()).Results)
				info += item.Title + Environment.NewLine;
			return info;
		}

	}
}
