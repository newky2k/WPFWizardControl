using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.WizardControl.Core
{
	/// <summary>
	/// Class WizardPageConfiguration.
	/// </summary>
	public class WizardPageConfiguration
    {
		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is hidden.
		/// </summary>
		/// <value><c>true</c> if this instance is hidden; otherwise, <c>false</c>.</value>
		public bool IsHidden { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance can go back.
		/// </summary>
		/// <value><c>true</c> if this instance can go back; otherwise, <c>false</c>.</value>
		public bool CanGoBack { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [hide buttons].
		/// </summary>
		/// <value><c>true</c> if [hide buttons]; otherwise, <c>false</c>.</value>
		public bool HideButtons { get; set; }

		/// <summary>
		/// Called when the page navigation is occuring
		/// </summary>
		/// <value>The navigation handler.</value>
		public Action<WizardNavigationEventArgs> NavigationHandler { get; set; }

		/// <summary>
		/// Gets or sets the on page shown handler.
		/// </summary>
		/// <value>The on page shown handler.</value>
		public Action<IWizardControl> OnPageShownHandler { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="WizardPageConfiguration"/> class.
		/// </summary>
		public WizardPageConfiguration()
        {
            CanGoBack = true;
            IsHidden = false;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="WizardPageConfiguration"/> class.
		/// </summary>
		/// <param name="title">The title.</param>
		public WizardPageConfiguration(string title) : this()
        {
            Title = title;
        }

    }
}
