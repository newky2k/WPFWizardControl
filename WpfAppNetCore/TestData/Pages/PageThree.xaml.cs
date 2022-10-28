using Dsoft.WizardControl.WPF;
using DSoft.WizardControl.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppNetCore.TestData.Pages
{
    /// <summary>
    /// Interaction logic for PageThree.xaml
    /// </summary>
    public partial class PageThree : UserControl, IWizardPage
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

        public WizardPageConfiguration PageConfig => new WizardPageConfiguration("Select the databases") { CanGoBack = true, NavigationHandler = NavigationHandler, OnPageShownHandler = OnShown};

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
            wizard.UpdateButtonVisibility(WizardButtonVisibility.Hidden, WizardButtons.Process, WizardButtons.Cancel, WizardButtons.Previous);
            wizard.UpdateButtonVisibility(WizardButtonVisibility.Visible, WizardButtons.Complete);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
