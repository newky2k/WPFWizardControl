using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.WizardControl.Core
{
    public interface IWizardControl
    {
        void Navigate(NavigationDirection direction);

        void SetButtonVisibility(WizardButtonVisibility visibility, params WizardButtons[] buttons);
    }
}
