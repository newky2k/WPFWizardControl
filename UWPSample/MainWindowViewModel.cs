using DSoft.WizardControl.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using UWPSample.TestData;
using UWPSample.TestData.Pages;

namespace UWPSample
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
        private IWizardPage _selectedPage;
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

        public IWizardPage SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                _selectedPage = value; NotifyPropertyChanged(nameof(SelectedPage));

                if (_selectedPage is PageTwo)
                {
                    NextTitle = "Go";
                }
                else
                {
                    NextTitle = "Foward";
                }
            }
        }

        private string _nextTitle = "Forward";

        public string NextTitle
        {
            get { return _nextTitle; }
            set { _nextTitle = value; NotifyPropertyChanged(nameof(NextTitle)); }
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

        public SharedViewModel SharedViewModel
        {
            get { return _sharedViewModel; }
            set { _sharedViewModel = value; }
        }

        #endregion

        public MainWindowViewModel(IWizardControl wizard)
        {
            Title = "Create Supplier";


            SharedViewModel = new SharedViewModel(wizard);

            CompletePage = new CompletePageView(SharedViewModel);
            ErrorPage = new ErrorPage(SharedViewModel);
            ProcessPage = new ProcessingPage(SharedViewModel);

            Pages = new ObservableCollection<IWizardPage>()
            {
                new PageOne(SharedViewModel, wizard),
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
