using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.WizardControl.Core
{
	/// <summary>
	/// Enum WizardStage
	/// </summary>
	public enum WizardStage
    {
        /// <summary>
        /// Setting up the data
        /// </summary>
        Setup,
        /// <summary>
        /// Show working/Progress screen
        /// </summary>
        Working,
        /// <summary>
        /// Process in complete
        /// </summary>
        Complete,
        /// <summary>
        /// Displays the error page
        /// </summary>
        Error,
    }
}
