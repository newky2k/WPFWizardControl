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
    /// Interaction logic for DefaultCompleteView.xaml
    /// </summary>
    public class DefaultCompleteView : UserControl, IWizardPage
    {
        public DefaultCompleteView()
        {
            var grd = new Grid();
            grd.Children.Add(new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "The Wizard has finished...",
            });

            this.Content = grd;
        }

        public List<KeyValuePair<string, object>> Parameters { get => new List<KeyValuePair<string, object>>(); set => Console.WriteLine(""); }

        public WizardPageConfiguration PageConfig => new WizardPageConfiguration("Wizard Complete");

		public Task<bool> ValidateAsync()
		{
			return Task.FromResult(true);
		}
	}
}
