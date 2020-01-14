using Dsoft.WizardControl.WPF;
using DSoft.WizardControl.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Dsoft.WizardControl.WPF
{
    /// <summary>
    /// Base Wizard ViewModel class
    /// </summary>
    internal class WizardControlViewModel : ViewModel
    {
        #region Events

        public event EventHandler<IWizardPage> OnSelectedPageChanged = delegate { };

        #endregion
        #region Fields

        private int mSelectedIndex;
        private List<KeyValuePair<String, Object>> mParameters;
        private String mHeading;
        private ObservableCollection<IWizardPage> _pages;
        private IWizardPage _progressPage;
        private IWizardPage _completePage;
        private string _title;
        private string _closeButtonTitle;
        private string _processButtonTitle;
        private string _cancelButtonTitle;
        private string _nextButtonTitle;
        private string _previousButtonTitle;
        private WizardStage _currentStage = WizardStage.Setup;
        private IWizardPage _selectedPage;

        #endregion

        #region Properties

        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged(nameof(Title)); }
        }

        public string ProcessButtonTitle
        {
            get { return _processButtonTitle; }
            set { _processButtonTitle = value; NotifyPropertyChanged(nameof(ProcessButtonTitle)); }
        }

        public string CloseButtonTitle
        {
            get { return _closeButtonTitle; }
            set { _closeButtonTitle = value; NotifyPropertyChanged(nameof(CloseButtonTitle)); }
        }

        public string CancelButtonTitle
        {
            get { return _cancelButtonTitle; }
            set { _cancelButtonTitle = value; NotifyPropertyChanged(nameof(CancelButtonTitle)); }
        }

        public string NextButtonTitle
        {
            get { return _nextButtonTitle; }
            set { _nextButtonTitle = value; NotifyPropertyChanged(nameof(NextButtonTitle)); }
        }

        public string PreviousButtonTitle
        {
            get { return _previousButtonTitle; }
            set { _previousButtonTitle = value; NotifyPropertyChanged(nameof(PreviousButtonTitle)); }
        }

        /// <summary>
        /// Whats is the current selected index
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return mSelectedIndex;
            }
            set
            {
                mSelectedIndex = value;

                NotifyPropertyChanged("SelectedIndex");

            }
        }

        /// <summary>
        /// Gets the current page
        /// </summary>
        public IWizardPage SelectedPage
        {
            get { return _selectedPage; }
            set { _selectedPage = value; NotifyPropertyChanged(nameof(SelectedPage)); OnSelectedPageChanged?.Invoke(this, _selectedPage); }
        }


        /// <summary>
        /// Is previous button enabled
        /// </summary>
        public Boolean PreviousEnabled
        {
            get
            {
                switch (_currentStage)
                {
                    case WizardStage.Working:
                        return false;

                    case WizardStage.Complete:
                        return false;

                    default:
                        return SelectedIndex > 0;
                }

                
            }
        }

        /// <summary>
        /// Is next button enabled
        /// </summary>
        public Boolean NextEnabled
        {
            get
            {
                if (Pages.Count == 0 || ActivePages.Count == 0)
                    return false;

                switch (_currentStage)
                {
                    case WizardStage.Working:
                        return false;

                    case WizardStage.Complete:
                        return false;
                    case WizardStage.Error:
                        return false;

                    default:
                        return SelectedIndex != ActivePages.Max(x => x.Key);
                }

                
            }
        }

        /// <summary>
        /// Is finish button enabled
        /// </summary>
        public Boolean ProcessEnabled
        {
            get
            {
                if (Pages.Count == 0 || ActivePages.Count == 0)
                    return false;

                switch (_currentStage)
                {
                    case WizardStage.Working:
                        return false;

                    case WizardStage.Complete:
                        return false;

                    default:
                        return SelectedIndex == ActivePages.Max(x => x.Key);
                }

                
            }
        }

        /// <summary>
        /// Is previous button enabled
        /// </summary>
        public Boolean CancelEnabled
        {
            get
            {
                switch (_currentStage)
                {
                    case WizardStage.Setup:
                        return true;
                    case WizardStage.Error:
                        return true;
                    default:
                        return false;
                }

            }
            //get { return mCancelEnabled; }
            //set
            //{
            //    mCancelEnabled = value;

            //    NotifyPropertyChanged("CancelEnabled");
            //}
        }

        public Boolean CompleteEnabled
        {
            get
            {
                switch (_currentStage)
                {
                    case WizardStage.Complete:
                        return true;

                    default:
                        return false;
                }


            }
        }

        /// <summary>
        /// Gets the pages for the wizard
        /// </summary>
        public ObservableCollection<IWizardPage> Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new ObservableCollection<IWizardPage>();

                return _pages;
            }
            set
            {

                _pages = value;

                
                //mPages.CollectionChanged += mPages_CollectionChanged;
                NotifyPropertyChanged("Pages");

                if (Pages != null)
                {
                    if (!Pages.Contains(ProgressPage))
                        Pages.Add(ProgressPage);

                    if (!Pages.Contains(CompletePage))
                        Pages.Add(CompletePage);

                    if (!Pages.Contains(ErrorPage))
                        Pages.Add(ErrorPage);

                    foreach (IWizardPage NewPage in Pages)
                    {
                        NewPage.Parameters = mParameters;
                    }

                    
                }

                if (Pages?.Count > 0)
                    SelectedPage = Pages[0];

                RecalculateNavigation();
            }
        }

        public Dictionary<int,IWizardPage> ActivePages
        {
            get
            {
                var aDict = new Dictionary<int, IWizardPage>();

                var aPages = Pages.Where(x => x.PageConfig.IsHidden.Equals(false) && (!x.Equals(ProgressPage) && !x.Equals(CompletePage) && !x.Equals(ErrorPage)));

                if (aPages.Any())
                {
                    foreach (var aPage in aPages)
                    {
                        aDict.Add(Pages.IndexOf(aPage), aPage);
                    }
                }
                   

                return aDict;
            }
        }
        /// <summary>
        /// Parameter list for communicating parameters / variables between pages.
        /// </summary>
        public List<KeyValuePair<String, Object>> Parameters
        {
            get { return mParameters; }
            set { mParameters = value; }
        }

        /// <summary>
        /// Current heading of the wizard
        /// </summary>
        public String SubTitle
        {
            get
            {
                if (mHeading == null)
                {
                    if (Pages != null && Pages.Count != 0)
                    {
                        mHeading = Pages[0].PageConfig.Title;
                    }
                    else
                    {
                        mHeading = String.Empty;
                    }

                }

                return mHeading;
            }
            set
            {
                if (mHeading != value)
                {
                    mHeading = value;

                    NotifyPropertyChanged("SubTitle");
                }
            }
        }

        public IWizardPage ProgressPage
        {
            get
            {
                if (_progressPage == null)
                    _progressPage = new DefaultProgressView();

                return _progressPage;
            }
            set { _progressPage = value; NotifyPropertyChanged(nameof(ProgressPage)); }
        }

        public IWizardPage CompletePage
        {
            get
            {
                if (_completePage == null)
                    _completePage = new DefaultCompleteView();

                return _completePage;
            }
            set { _completePage = value; NotifyPropertyChanged(nameof(CompletePage)); }
        }

        private IWizardPage _errorPage;

        public IWizardPage ErrorPage
        {
            get
            {
                if (_errorPage == null)
                    _errorPage = new DefaultErrorView();

                return _errorPage;
            }
            set { _errorPage = value; NotifyPropertyChanged(nameof(ErrorPage)); }
        }


        public Visibility CompleteButtonVisibility
        {
            get
            {
                return (CompleteEnabled) ? Visibility.Visible : Visibility.Collapsed;
            }

        }

        public Visibility CancelButtonVisibility
        {
            get
            {
                return (CancelEnabled) ? Visibility.Visible : Visibility.Hidden;
            }

        }

        public Visibility ProcessButtonVisibility
        {
            get
            {
                return (ProcessEnabled) ? Visibility.Visible : Visibility.Collapsed;
            }
           
        }

        public Visibility NextButtonVisibility
        {
            get { return (NextEnabled) ? Visibility.Visible : Visibility.Collapsed; }

        }

        public Visibility PreviousButtonVisibility
        {
            get { return (PreviousEnabled) ? Visibility.Visible : Visibility.Collapsed; }

        }

        #region Commands




        /// <summary>
        /// Gets the previous command.
        /// </summary>
        /// <value>
        /// The previous command.
        /// </value>
        public ICommand PreviousCommand
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the next command.
        /// </summary>
        /// <value>
        /// The next command.
        /// </value>
        public ICommand NextCommand
        {
            get;
            internal set;
        }

        public ICommand ProcessButtonCommand
        {
            get;
            internal set;
        }

        internal Func<Task<WizardProcessResult>> ProcessFunction { get; set; }

        internal Action CloseFunction { get; set; }

        internal Action CancelFunction { get; set; }

        /// <summary>
        /// Command to be called when the wizard completes.  Should be set in the View to be called by the view model
        /// </summary>
        public ICommand CompleteCommand { get; internal set; }

        public ICommand CancelCommand { get; internal set; }

        private int LastActivePageIndex
        {
            get
            {
                return ActivePages.Max(x => x.Key);
            }
        }
        #endregion

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardControlViewModel"/> class.
        /// </summary>
        public WizardControlViewModel()
        {

            this.Pages = new ObservableCollection<IWizardPage>();
            this.Parameters = new List<KeyValuePair<string, object>>();

            PreviousCommand = new DelegateCommand(() =>
            {
                if (SelectedIndex == Pages.IndexOf(ErrorPage))
                {
                    SetPage(LastActivePageIndex);
                }
                else
                {
                    SetPage(GetPreviousPageIndex(SelectedIndex));
                }
                

            });

            NextCommand = new DelegateCommand(() =>
            {
                var cuItem = this.Pages[SelectedIndex];

                if (cuItem.Validate())
                {
                    SetPage(GetNextPageIndex(SelectedIndex));
                }

            });

            ProcessButtonCommand = new DelegateCommand(() => 
            {
                var cuItem = this.Pages[SelectedIndex];

                if (cuItem.Validate())
                {

                    IsBusy = true;

                    Task.Run(async () =>
                    {
                        SetPage(Pages.IndexOf(ProgressPage));

                        try
                        {
                            
                            if (ProcessFunction != null)
                            {
                                var result = await ProcessFunction();

                                switch (result)
                                {
                                    case WizardProcessResult.Complete:
                                        {
                                            SetPage(Pages.IndexOf(CompletePage));
                                        }
                                        break;
                                    default:
                                        {
                                            SetPage(Pages.IndexOf(ErrorPage));
                                        }
                                        break;
                                }
                            }
                           else
                            {
                                SetPage(Pages.IndexOf(CompletePage));
                            }
                            
                        }
                        catch (Exception)
                        {
                            
                        }
                        

                        
                    });

                    

                    

                    //

                    IsBusy = false;
                }


            });

            CompleteCommand = new DelegateCommand(() => 
            {
                CloseFunction?.Invoke();
            });

            CancelCommand = new DelegateCommand(() => 
            {
                CancelFunction?.Invoke();
            });

        }


        #endregion

        #region Methods
        internal void SetPage(int newIndex)
        {
            if (newIndex == Pages.IndexOf(ProgressPage))
            {
                StartProcessingStage();
            }
            else if (newIndex == Pages.IndexOf(CompletePage))
            {
                StartCompletionStage();
            }
            else if (newIndex == Pages.IndexOf(ErrorPage))
            {
                StartErrorStage();
            }
            else
            {
                StartSetupStage();
            }

            this.SelectedIndex = newIndex;
            this.SelectedPage = Pages[this.SelectedIndex];

            this.SubTitle = Pages[this.SelectedIndex].PageConfig.Title;

            RecalculateNavigation();
        }

        public void StartProcessingStage()
        {
            _currentStage = WizardStage.Working;
        }

        public void StartCompletionStage()
        {
            _currentStage = WizardStage.Complete;
        }

        public void StartSetupStage()
        {
            _currentStage = WizardStage.Setup;
        }

        public void StartErrorStage()
        {
            _currentStage = WizardStage.Error;
        }

        private int GetPreviousPageIndex(int currentIndex)
        {
            if (currentIndex == 0)
                return currentIndex;

            var newIndex = currentIndex - 1;


            if (Pages[newIndex].PageConfig.IsHidden)
                return GetPreviousPageIndex(newIndex);

            return newIndex;
        }

        private int GetNextPageIndex(int currentIndex)
        {
            if (currentIndex == ActivePages.Max(x => x.Key))
                return currentIndex;

            var newIndex = currentIndex + 1;

            if (Pages[newIndex].PageConfig.IsHidden)
                return GetNextPageIndex(newIndex);

            return newIndex;
        }
        public virtual void DidFinish()
        {

        }

        public void RecalculateNavigation()
        {

            NotifyPropertyChanged(nameof(ProcessEnabled));
            NotifyPropertyChanged(nameof(NextEnabled));
            NotifyPropertyChanged(nameof(PreviousEnabled));
            NotifyPropertyChanged(nameof(CompleteEnabled));
            NotifyPropertyChanged(nameof(CancelEnabled));


            NotifyPropertyChanged(nameof(ProcessButtonVisibility));
            NotifyPropertyChanged(nameof(NextButtonVisibility));
            NotifyPropertyChanged(nameof(PreviousButtonVisibility));
            NotifyPropertyChanged(nameof(CancelButtonVisibility));
            NotifyPropertyChanged(nameof(CompleteButtonVisibility));
        }
        /// <summary>
        /// Set the Parameters property of the Pages added to be that of this class. ie centralise them to point here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mPages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (IWizardPage NewPage in e.NewItems)
            {
                NewPage.Parameters = mParameters;
            }

            RecalculateNavigation();

        }

        #endregion
    }
}
