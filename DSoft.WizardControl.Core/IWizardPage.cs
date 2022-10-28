using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.WizardControl.Core
{
    public interface IWizardPage
    {
        WizardPageConfiguration PageConfig { get; }

        Task<bool> ValidateAsync();
    }
}
