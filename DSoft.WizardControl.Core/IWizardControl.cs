using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.WizardControl.Core
{
    public interface IWizardControl
    {
        void Navigate(NavigationDirection direction);

        void UpdateButtonVisibility(WizardButtonVisibility visibility, params WizardButtons[] buttons);

        [Obsolete("Use UpdateButtonVisibility instead")]
        void SetButtonVisibility(WizardButtonVisibility visibility, params WizardButtons[] buttons);
    }
}
