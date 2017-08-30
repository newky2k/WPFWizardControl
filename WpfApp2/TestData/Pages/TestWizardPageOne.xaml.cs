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
    public partial class TestWizardPageOne : WizardPage
    {
       
        
        public TestWizardPageOne()
        {
            InitializeComponent();

            
        }

        public override BaseWizardPageViewModel ViewModel
        {
            get
            {
                return new TestPageOneViewModel();
            }
        }

    }
}
