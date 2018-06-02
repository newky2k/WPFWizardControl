using Dsoft.WizardControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.TestData.Pages.ViewModels
{
    public class TestPageOneViewModel : BaseWizardPageViewModel
    {
        public override bool Validate()
        {
           return true;
        }

        public TestPageOneViewModel()
        {
            Title = "Enter the details of the Job";
        }
    }
}
