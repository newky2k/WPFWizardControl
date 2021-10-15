using DSoft.WizardControl.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPSample.TestData.Pages
{
    public sealed partial class PageThree : UserControl, IWizardPage
    {
        private SharedViewModel _viewModel;

        public SharedViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; DataContext = _viewModel; }
        }

        public PageThree()
        {
            InitializeComponent();
        }

        public WizardPageConfiguration PageConfig => new WizardPageConfiguration("Select the databases") { CanGoBack = true, NavigationHandler = NavigationHandler, OnPageShownHandler = OnShown };

        public List<KeyValuePair<string, object>> Parameters { get => new List<KeyValuePair<string, object>>(); set => Console.WriteLine(""); }

        public bool Validate()
        {
            return true;
        }

        public void NavigationHandler(DSoft.WizardControl.Core.WizardNavigationEventArgs evts)
        {
            switch (evts.Direction)
            {
                case NavigationDirection.Backwards:
                    {

                    }
                    break;
                case NavigationDirection.Forward:
                    {

                    }
                    break;
            }
        }

        public void OnShown(IWizardControl wizard)
        {
            wizard.UpdateButtonVisibility(WizardButtonVisibility.Hidden, WizardButtons.Process, WizardButtons.Cancel, WizardButtons.Previous);
            wizard.UpdateButtonVisibility(WizardButtonVisibility.Visible, WizardButtons.Complete);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
