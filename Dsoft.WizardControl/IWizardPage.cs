using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsoft.WizardControl.WPF
{
    public interface IWizardPage
    {
        string Title { get; }

        List<KeyValuePair<String, Object>> Parameters { get; set; }

        bool Validate();

        //IWizardPageViewModel ViewModel { get; }


    }
}
