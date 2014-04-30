using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Internal
{
    /// <summary>
    /// Manages HardwareButtons_BackPressed for WinPhone pages
    /// </summary>
    internal class NavigationHelper : DependencyObject
    {
        private Page Page { get; set; }
        private Frame Frame { get { return Page.Frame; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationHelper"/> class.
        /// </summary>
        public NavigationHelper(Page page)
        {
            Page = page;

            Page.Loaded += (sender, e) =>
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonsOnBackPressed;
            };

            // Undo the same changes when the page is no longer visible
            Page.Unloaded += (sender, e) =>
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtonsOnBackPressed;
            };
        }


        RelayCommand _goBackCommand;
        RelayCommand _goForwardCommand;

        /// <summary>
        /// <see cref="RelayCommand"/> used to bind to the back Button's Command property
        /// for navigating to the most recent item in back navigation history, if a Frame
        /// manages its own navigation history.
        /// 
        /// The <see cref="RelayCommand"/> is set up to use the virtual method <see cref="GoBack"/>
        /// as the Execute Action and <see cref="CanGoBack"/> for CanExecute.
        /// </summary>
        public RelayCommand GoBackCommand
        {
            get { return _goBackCommand ?? (_goBackCommand = new RelayCommand(GoBack, CanGoBack)); }
        }

        /// <summary>
        /// <see cref="RelayCommand"/> used for navigating to the most recent item in 
        /// the forward navigation history, if a Frame manages its own navigation history.
        /// 
        /// The <see cref="RelayCommand"/> is set up to use the virtual method <see cref="GoForward"/>
        /// as the Execute Action and <see cref="CanGoForward"/> for CanExecute.
        /// </summary>
        public RelayCommand GoForwardCommand
        {
            get { return _goForwardCommand ?? (_goForwardCommand = new RelayCommand(GoForward, CanGoForward)); }
        }

        /// <summary>
        /// Virtual method used by the <see cref="GoBackCommand"/> property
        /// to determine if the <see cref="Frame"/> can go back.
        /// </summary>
        /// <returns>
        /// true if the <see cref="Frame"/> has at least one entry 
        /// in the back navigation history.
        /// </returns>
        public virtual bool CanGoBack()
        {
            return Frame != null && Frame.CanGoBack;
        }
        /// <summary>
        /// Virtual method used by the <see cref="GoForwardCommand"/> property
        /// to determine if the <see cref="Frame"/> can go forward.
        /// </summary>
        /// <returns>
        /// true if the <see cref="Frame"/> has at least one entry 
        /// in the forward navigation history.
        /// </returns>
        public virtual bool CanGoForward()
        {
            return Frame != null && Frame.CanGoForward;
        }

        /// <summary>
        /// Virtual method used by the <see cref="GoBackCommand"/> property
        /// to invoke the <see cref="Windows.UI.Xaml.Controls.Frame.GoBack"/> method.
        /// </summary>
        public virtual void GoBack()
        {
            if (Frame != null && Frame.CanGoBack) Frame.GoBack();
        }
        /// <summary>
        /// Virtual method used by the <see cref="GoForwardCommand"/> property
        /// to invoke the <see cref="Windows.UI.Xaml.Controls.Frame.GoForward"/> method.
        /// </summary>
        public virtual void GoForward()
        {
            if (Frame != null && Frame.CanGoForward) Frame.GoForward();
        }

        /// <summary>
        /// Invoked when the hardware back button is pressed. For Windows Phone only.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        private void HardwareButtonsOnBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (GoBackCommand.CanExecute(null))
            {
                e.Handled = true;
                GoBackCommand.Execute(null);
            }
        }
    }
}
