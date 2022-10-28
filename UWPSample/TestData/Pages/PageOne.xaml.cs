using DSoft.WizardControl.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class PageOne : UserControl, IWizardPage
    {

        private SharedViewModel _viewModel;
        private IWizardControl _wizardControl;

        public SharedViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; DataContext = _viewModel; }
        }

        public PageOne(SharedViewModel viewModel, IWizardControl wizardControl)
        {
            InitializeComponent();

            ViewModel = viewModel;

            _wizardControl = wizardControl;
        }

        public WizardPageConfiguration PageConfig => new WizardPageConfiguration("Enter the accounts information") { OnPageShownHandler = OnShown };

        public List<KeyValuePair<string, object>> Parameters { get => new List<KeyValuePair<string, object>>(); set => Console.WriteLine(""); }

        public void OnShown(IWizardControl wizard)
        {
            //wizard.SetButtonVisibility(WizardButtonVisibility.Hidden, WizardButtons.All);

        }

		public Task<bool> ValidateAsync()
		{
			ViewModel.ValidateAllProperties();

            return Task.FromResult(!ViewModel.HasErrors);
		}

    }
}
