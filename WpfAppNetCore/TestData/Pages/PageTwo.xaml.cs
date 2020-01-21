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
    /// Interaction logic for PageTwo.xaml
    /// </summary>
    public partial class PageTwo : UserControl, IWizardPage
    {
        private SharedViewModel _viewModel;

        public SharedViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; DataContext = _viewModel; }
        }

        public PageTwo()
        {
            InitializeComponent();
        }

        public bool IsHidden => ViewModel.HidePage2;

        public WizardPageConfiguration PageConfig => new WizardPageConfiguration("Enter the banking information")
                                                {
                                                    IsHidden = ViewModel.HidePage2,
                                                    HideButtons = true,
                                                    NavigationHandler = NavigationHandler
                                                };

        public string Title => "Enter the banking information";

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
    }
}
