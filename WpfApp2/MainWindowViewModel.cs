using Dsoft.WizardControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp2.TestData.Pages;

namespace WpfApp2
{
    public class MainWindowViewModel : ViewModel
    {
        private string _title;
        private ObservableCollection<IWizardPage> _pages;

        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged("Title"); }
        }

        public ObservableCollection<IWizardPage> Pages
        {
            get { return _pages; }
            set { _pages = value; NotifyPropertyChanged("Pages"); }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MessageBox.Show("Bye!");
                });
            }
        }

        public MainWindowViewModel()
        {
            Title = "Test This";

            Pages = new ObservableCollection<IWizardPage>()
            {
                new TestWizardPageOne(),
                new TestWizardPageOne(),
            };
        }
    }
}
