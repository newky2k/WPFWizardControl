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
		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultErrorView"/> class.
		/// </summary>
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

		/// <summary>
		/// Gets the page configuration.
		/// </summary>
		/// <value>The page configuration.</value>
		public WizardPageConfiguration PageConfig => new WizardPageConfiguration("Wizard is doing stuff");

		/// <summary>
		/// Gets or sets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		public List<KeyValuePair<string, object>> Parameters { get => new List<KeyValuePair<string, object>>(); set => Console.WriteLine(""); }

		/// <summary>
		/// Validates the asynchronously.
		/// </summary>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public Task<bool> ValidateAsync()
		{
			return Task.FromResult(true);
		}
	}
}
