using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.WizardControl.Core
{
    public interface IWizardControl
    {
        List<IWizardPage> AvailablePages { get; }

        void Navigate(NavigationDirection direction);

        void UpdateButtonVisibility(WizardButtonVisibility visibility, params WizardButtons[] buttons);

        void UpdateStage(WizardStage stage);

        void RecalculateNavigation();
    }
}
