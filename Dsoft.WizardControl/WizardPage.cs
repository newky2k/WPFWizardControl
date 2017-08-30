using Dsoft.WizardControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dsoft.WizardControl.WPF
{
    public abstract class WizardPage : UserControl
    {

        public abstract BaseWizardPageViewModel ViewModel { get; }

        public WizardPage() : base()
        {
            
        }
    }

}
