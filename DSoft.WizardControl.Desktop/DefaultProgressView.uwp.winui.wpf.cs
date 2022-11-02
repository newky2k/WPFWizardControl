using DSoft.WizardControl.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if UAP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#elif WPF
using System.Windows.Controls;
using System.Windows;
#else
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#endif

namespace DSoft.WizardControl
{
    public class DefaultProgressView : UserControl, IWizardPage
    {
        public DefaultProgressView()
        {
            var grd = new Grid();
            grd.Children.Add(new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Wizard is Working...",
            });

            this.Content = grd;
        }

        public WizardPageConfiguration PageConfig => new WizardPageConfiguration("Wizard is doing stuff");

        public List<KeyValuePair<string, object>> Parameters { get => new List<KeyValuePair<string, object>>(); set => Console.WriteLine(""); }

		public Task<bool> ValidateAsync()
		{
			return Task.FromResult(true);
		}
	}
}
