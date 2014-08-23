using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Portal;
using Esri.ArcGISRuntime.Security;
using Esri.ArcGISRuntime.Toolkit.Controls;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SignInDialogSample : Page
	{
		public const string PortalUrl = "http://www.arcgis.com/sharing/rest";
		public const string PortalIWAUrl = "https://portaliwa.esri.com/gis/sharing/rest";
		public const string SecuredServiceUrl = "http://serverapps10.esri.com/arcgis/rest/services/GulfLawrenceSecureUser1/MapServer";

		public SignInDialogSample()
		{
			this.InitializeComponent();
			IdentityManager.Current.ChallengeMethod = new SignInDialog() { Style = Resources[typeof(SignInDialog)] as Style }.CreateCredentialAsync;
			//IdentityManager.Current.ChallengeMethod = new SignInDialog().CreateCredentialAsync;
			//var signInDialog = (Resources["MySignInDialog"] as SignInDialog);
			//IdentityManager.Current.ChallengeMethod = signInDialog.CreateCredentialAsync;
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{

			//IdentityManager.Current.ChallengeMethod = MySignInDialog.CreateCredentialAsync;
		}

		private async void PortalSignInOnClick(object sender, RoutedEventArgs e)
		{
			var im = IdentityManager.Current;
			string message;
			try
			{
				var cancellationToken = new CancellationTokenSource(20000).Token;
				await im.GetCredentialAsync(new CredentialRequestInfo { ServiceUri = PortalUrl, CancellationToken = cancellationToken}, true);
				var portal = await ArcGISPortal.CreateAsync(new Uri(PortalUrl));
				var user = portal.CurrentUser;
				message = "User: " + (user == null ? "" : user.FullName);

			}
			catch (Exception ex)
			{
				message = "Error:" + ex.Message;
			}
			await new MessageDialog(message).ShowAsync();
		}

		private async void IWAPortalSignInOnClick(object sender, RoutedEventArgs e)
		{
			var im = IdentityManager.Current;
			string message;
			try
			{
				var crd = await im.GetCredentialAsync(new CredentialRequestInfo { ServiceUri = PortalIWAUrl, AuthenticationType = AuthenticationType.NetworkCredential}, true);
				//var portal = await ArcGISPortal.CreateAsync(new Uri(PortalIWAUrl));
				//var user = portal.CurrentUser;
				//message = "User: " + user.FullName;
				message = "OK";
			}
			catch (Exception ex)
			{
				message = "Error:" + ex.Message;
			}
			await new MessageDialog(message).ShowAsync();
			//var portal = await ArcGISPortal.CreateAsync(new Uri(PortalIWAUrl));
			//var user = portal.CurrentUser;
		}
		private async void AccessSecuredServiceOnClick(object sender, RoutedEventArgs e)
		{
			var layer = new ArcGISDynamicMapServiceLayer {ServiceUri = SecuredServiceUrl};
			await layer.InitializeAsync();
		}
		private void SignOutOnClick(object sender, RoutedEventArgs e)
		{
			var im = IdentityManager.Current;
			foreach(var crd in im.Credentials)
				im.RemoveCredential(crd);
		}
	}
}
