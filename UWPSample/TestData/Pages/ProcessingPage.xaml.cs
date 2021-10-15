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
    public sealed partial class ProcessingPage : UserControl, IWizardPage
    {

        private SharedViewModel _viewModel;


        public SharedViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; DataContext = _viewModel; }
        }

        public ProcessingPage(SharedViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }

        public string Title => "Wizard is Doing something";


        public WizardPageConfiguration PageConfig => new WizardPageConfiguration("Wizard is Doing something") { };


        public List<KeyValuePair<string, object>> Parameters { get => new List<KeyValuePair<string, object>>(); set => Console.WriteLine(""); }

        public bool Validate()
        {
            return true;
        }
    }
}
