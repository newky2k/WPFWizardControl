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
	/// <summary>
	/// Class DefaultCompleteView.
	/// Implements the <see cref="UserControl" />
	/// Implements the <see cref="IWizardPage" />
	/// </summary>
	/// <seealso cref="UserControl" />
	/// <seealso cref="IWizardPage" />
	public class DefaultCompleteView : UserControl, IWizardPage
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultCompleteView"/> class.
		/// </summary>
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

		/// <summary>
		/// Gets or sets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		public List<KeyValuePair<string, object>> Parameters { get => new List<KeyValuePair<string, object>>(); set => Console.WriteLine(""); }

		/// <summary>
		/// Gets the page configuration.
		/// </summary>
		/// <value>The page configuration.</value>
		public WizardPageConfiguration PageConfig => new WizardPageConfiguration("Wizard Complete");

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
