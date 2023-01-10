using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.WizardControl.Core
{
	/// <summary>
	/// Interface IWizardPage
	/// </summary>
	public interface IWizardPage
    {
		/// <summary>
		/// Gets the page configuration.
		/// </summary>
		/// <value>The page configuration.</value>
		WizardPageConfiguration PageConfig { get; }

		/// <summary>
		/// Validates the asynchronously.
		/// </summary>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		Task<bool> ValidateAsync();
    }
}
