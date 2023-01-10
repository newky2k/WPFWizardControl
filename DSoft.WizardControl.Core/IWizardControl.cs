using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.WizardControl.Core
{
	/// <summary>
	/// Interface IWizardControl
	/// </summary>
	public interface IWizardControl
    {
		/// <summary>
		/// Gets the available pages.
		/// </summary>
		/// <value>The available pages.</value>
		List<IWizardPage> AvailablePages { get; }

		/// <summary>
		/// Navigates in the specified direction.
		/// </summary>
		/// <param name="direction">The direction.</param>
		void Navigate(NavigationDirection direction);

		/// <summary>
		/// Updates the button visibility.
		/// </summary>
		/// <param name="visibility">The visibility.</param>
		/// <param name="buttons">The buttons.</param>
		void UpdateButtonVisibility(WizardButtonVisibility visibility, params WizardButtons[] buttons);

		/// <summary>
		/// Updates the stage.
		/// </summary>
		/// <param name="stage">The stage.</param>
		void UpdateStage(WizardStage stage);

		/// <summary>
		/// Recalculates the navigation.
		/// </summary>
		void RecalculateNavigation();
    }
}
