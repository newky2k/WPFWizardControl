using DSoft.WizardControl.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUISample.TestData.Pages
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

        public Task<bool> ValidateAsync()
        {
            return Task.FromResult(true);
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
            wizard.UpdateButtonVisibility(WizardButtonVisibility.Collapsed, WizardButtons.Process, WizardButtons.Cancel);
            wizard.UpdateButtonVisibility(WizardButtonVisibility.Visible, WizardButtons.Complete);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
