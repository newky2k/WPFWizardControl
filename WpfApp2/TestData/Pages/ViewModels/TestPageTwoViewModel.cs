using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.TestData.Pages.ViewModels
{
    public class TestPageTwoViewModel : ViewModel
    {
        private List<KeyValuePair<string, object>> _parameters;

        public string Title => "Test Page 2";

        public List<KeyValuePair<string, object>> Parameters { get => _parameters; set => _parameters = value; }

        public bool Validate()
        {
            return true;
        }

        public TestPageTwoViewModel()
        {

        }
    }
}
