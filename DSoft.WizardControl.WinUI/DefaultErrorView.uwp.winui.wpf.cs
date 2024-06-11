using DSoft.WizardControl.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WPF
using System.Windows.Controls;
using System.Windows;
#else
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#endif
namespace DSoft.WizardControl
{
	/// <summary>
	/// Class DefaultErrorView.
	/// Implements the <see cref="UserControl" />
	/// Implements the <see cref="IWizardPage" />
	/// </summary>
	/// <seealso cref="UserControl" />
	/// <seealso cref="IWizardPage" />
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
