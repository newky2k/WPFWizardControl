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
using WpfApp2.TestData;
using WpfApp2.TestData.Pages;

namespace WpfApp2
{
    public class MainWindowViewModel : ViewModel
    {

        public event EventHandler<bool> OnRequestCloseWindow;

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

                    OnRequestCloseWindow?.Invoke(this, false);
                });
            }
        }

        public ICommand FinishCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MessageBox.Show("Fin!");

                    OnRequestCloseWindow?.Invoke(this, false);
                });
            }
        }

        private SharedViewModel _sharedViewModel;

        public SharedViewModel SharedViewModel
        {
            get { return _sharedViewModel; }
            set { _sharedViewModel = value; }
        }


        public MainWindowViewModel()
        {
            Title = "Create Supplier";

            SharedViewModel = new SharedViewModel();

            Pages = new ObservableCollection<IWizardPage>()
            {
                new PageOne(SharedViewModel),
                new PageTwo()
                {
                    ViewModel = SharedViewModel,
                },
                new PageThree()
                {
                    ViewModel = SharedViewModel,
                },
            };
        }
    }
}
