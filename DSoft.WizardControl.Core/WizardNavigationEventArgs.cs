using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.WizardControl.Core
{
	/// <summary>
	/// Class WizardNavigationEventArgs.
	/// </summary>
	public class WizardNavigationEventArgs
    {
		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		/// <value>The direction.</value>
		public NavigationDirection Direction { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="WizardNavigationEventArgs"/> is handled.
		/// </summary>
		/// <value><c>true</c> if handled; otherwise, <c>false</c>.</value>
		public bool Handled { get; set; }
    }
}
