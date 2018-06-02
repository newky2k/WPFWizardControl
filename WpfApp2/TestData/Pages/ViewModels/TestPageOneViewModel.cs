using Dsoft.WizardControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.TestData.Pages.ViewModels
{
    public class TestPageOneViewModel : ViewModel
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged("Title"); }
        }

        public List<KeyValuePair<string, object>> Parameters { get; set; }

        public bool Validate()
        {
           return true;
        }

        public TestPageOneViewModel()
        {
            Title = "Enter the details of the Job";
        }
    }
}
