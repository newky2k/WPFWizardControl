using DSoft.WizardControl.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if UAP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
#endif

namespace DSoft.WizardControl
{
    public class DefaultErrorView : UserControl, IWizardPage
    {
        public DefaultErrorView()
        {
            var grd = new Grid();
            grd.Children.Add(new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Ooops there was an error....",
            });

            this.Content = grd;
        }

        public WizardPageConfiguration PageConfig => new WizardPageConfiguration("Wizard is doing stuff");

        public List<KeyValuePair<string, object>> Parameters { get => new List<KeyValuePair<string, object>>(); set => Console.WriteLine(""); }

        public bool Validate()
        {
            return true;
        }
    }
}
