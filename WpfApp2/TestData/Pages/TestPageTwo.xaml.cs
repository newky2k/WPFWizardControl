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

namespace WpfApp2.TestData.Pages
{
    /// <summary>
    /// Interaction logic for TestPageTwo.xaml
    /// </summary>
    public partial class TestPageTwo : UserControl, IWizardPage
    {
        public TestPageTwo()
        {
            InitializeComponent();
        }

        public string Title => "Test Page 2";

        private List<KeyValuePair<string, object>> _parameters;

        public List<KeyValuePair<string, object>> Parameters { get => _parameters; set => _parameters = value; }

        public bool Validate()
        {
            return true;
        }
    }
}
