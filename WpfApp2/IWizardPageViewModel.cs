using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public interface IWizardPageViewModel
    {
        string Title { get; }

        WizardParameterSet ParameterSet { get; set; }

    }
}
