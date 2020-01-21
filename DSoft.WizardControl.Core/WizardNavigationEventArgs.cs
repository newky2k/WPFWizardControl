using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.WizardControl.Core
{
    public class WizardNavigationEventArgs
    {
        public NavigationDirection Direction { get; set; }

        public bool Handled { get; set; }
    }
}
