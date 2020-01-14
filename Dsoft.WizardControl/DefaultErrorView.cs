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

namespace Dsoft.WizardControl.WPF
{
    /// <summary>
    /// Interaction logic for DefaultErrorView.xaml
    /// </summary>
    public class DefaultErrorView : UserControl, IWizardPage
    {
        public DefaultErrorView()
        {
            var grd = new Grid();
            grd.Children.Add(new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Content = "Ooops there was an error....",
            });

            this.Content = grd;
        }

        public string Title => "Wizard is doing stuff";

        public bool IsHidden => false;

        public List<KeyValuePair<string, object>> Parameters { get => new List<KeyValuePair<string, object>>(); set => Console.WriteLine(""); }

        public bool Validate()
        {
            return true;
        }
    }
}
