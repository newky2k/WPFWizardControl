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
    /// Interaction logic for PageOne.xaml
    /// </summary>
    public partial class PageOne : UserControl, IWizardPage
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

        public WizardPageConfiguration PageConfig => new WizardPageConfiguration("Enter the accounts information"){OnPageShownHandler = OnShown};

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
