using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp2
{
    public abstract class WizardPage : UserControl
    {

        public abstract BaseWizardPageViewModel ViewModel { get; }

        public WizardPage() : base()
        {
            
        }
    }

}
