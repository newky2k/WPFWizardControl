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
        #region Fields
        private SharedViewModel _sharedViewModel;
        private ObservableCollection<IWizardPage> _pages;
        private string _title;
        private IWizardPage _completePage;
        private IWizardPage _errorPage;
        private IWizardPage _processingPage;
        #endregion
        public event EventHandler<bool> OnRequestCloseWindow;

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


        public IWizardPage CompletePage
        {
            get { return _completePage; }
            set { _completePage = value; NotifyPropertyChanged(nameof(CompletePage)); }
        }

        public IWizardPage ErrorPage
        {
            get { return _errorPage; }
            set { _errorPage = value; NotifyPropertyChanged(nameof(ErrorPage)); }
        }

        public IWizardPage ProcessPage
        {
            get { return _processingPage; }
            set { _processingPage = value; NotifyPropertyChanged(nameof(ProcessPage)); }
        }

        #region Functions
        public Action CloseFunction
        {
            get
            {
                return () =>
                {
                    OnRequestCloseWindow?.Invoke(this, false);
                };
            }
        }

        public Action CancelFunction
        {
            get
            {
                return () =>
                {

                    OnRequestCloseWindow?.Invoke(this, false);
                };
            }
        }

        public Func<Task<WizardProcessResult>> ProcessFunction
        {
            get
            {
                return () =>
                {
                    return ProcessAsync();
                };
            }
        }
        #endregion

        public SharedViewModel SharedViewModel
        {
            get { return _sharedViewModel; }
            set { _sharedViewModel = value; }
        }


        public MainWindowViewModel()
        {
            Title = "Create Supplier";

            SharedViewModel = new SharedViewModel();

            CompletePage = new CompletePageView(SharedViewModel);
            ErrorPage = new ErrorPage(SharedViewModel);
            ProcessPage = new ProcessingPage(SharedViewModel);

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

        public async Task<WizardProcessResult> ProcessAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            return WizardProcessResult.Complete;
        }
    }
}
