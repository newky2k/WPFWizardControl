using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsoft.WizardControl.WPF
{
    public interface IWizardPage
    {
        IWizardPageViewModel WizardPageViewModel { get; }


    }
}
