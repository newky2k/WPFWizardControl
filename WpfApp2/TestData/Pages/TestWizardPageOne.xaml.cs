using Dsoft.WizardControl.WPF;
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
using WpfApp2.TestData.Pages.ViewModels;

namespace WpfApp2.TestData.Pages
{
    /// <summary>
    /// Interaction logic for TestWizardPageOne.xaml
    /// </summary>
    public partial class TestWizardPageOne : UserControl, IWizardPage
    {

        private TestPageOneViewModel _viewModel;

        public TestPageOneViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; DataContext = _viewModel; }
        }

        public TestWizardPageOne()
        {
            InitializeComponent();

            ViewModel = new TestPageOneViewModel();
        }


        public string Title
        {
            get { return ViewModel.Title; }
        }

        public List<KeyValuePair<string, object>> Parameters { get => ViewModel.Parameters; set => ViewModel.Parameters = value; }

        public bool Validate()
        {
            return ViewModel.Validate();
        }
    }
}
