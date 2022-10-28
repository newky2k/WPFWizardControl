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
        public event EventHandler<int> OnSelectedIndexChanged = delegate { };
        #endregion

        #region Fields

        private int mSelectedIndex = 0;
        private ObservableCollection<IWizardPage> _pages;
        private IWizardPage _progressPage;
        private IWizardPage _completePage;
        private IWizardPage _errorPage;
        private string _title;
        private string _closeButtonTitle;
        private string _processButtonTitle;
        private string _cancelButtonTitle;
        private string _nextButtonTitle;
        private string _previousButtonTitle;
        private WizardStage _currentStage = WizardStage.Setup;
        private IWizardPage _selectedPage;
        private ProcessMode processMode;
        private Dictionary<WizardButtons, Visibility> _buttonVisibility = new Dictionary<WizardButtons, Visibility>();
        #endregion

        #region Properties

        internal IWizardControl WizardControl { get; set; }

        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged(nameof(Title)); }
        }

        public ProcessMode ProcessMode
        {
            get { return processMode; }
            set { processMode = value; NotifyPropertyChanged(nameof(ProcessMode)); }
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
                if (mSelectedIndex != value)
                {
                    mSelectedIndex = value;

                    NotifyPropertyChanged("SelectedIndex");

                    OnSelectedIndexChanged?.Invoke(this, mSelectedIndex);
                }
            }
        }

        /// <summary>
        /// Gets the current page
        /// </summary>
        public IWizardPage SelectedPage
        {
            get { return _selectedPage; }
            set { _selectedPage = value; NotifyPropertiesChanged(nameof(SelectedPage), nameof(SubTitle)); OnSelectedPageChanged?.Invoke(this, _selectedPage); }
        }

        /// <summary>
        /// Is previous button enabled
        /// </summary>
        public Boolean PreviousEnabled
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
                        {
                            if (SelectedPage?.PageConfig?.CanGoBack == false)
                                return false;

                            return SelectedIndex > 0;
                        }
                        
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

                if (ProcessMode == ProcessMode.None)
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
                if (Pages.Count == 0 || ActivePages.Count == 0)
                    return false;

                if (ProcessMode == ProcessMode.None)
                {
                    if (SelectedIndex == ActivePages.Max(x => x.Key))
                        return false;
                }
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
        }

        public Boolean CompleteEnabled
        {
            get
            {
                if (Pages.Count == 0 || ActivePages.Count == 0)
                    return false;

                if (ProcessMode == ProcessMode.None)
                {
                    if (SelectedIndex == ActivePages.Max(x => x.Key))
                        return true;
                }

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
        /// Current heading of the wizard
        /// </summary>
        public String SubTitle
        {
            get
            {
                if (Pages != null && Pages.Count != 0)
                {
                    if (SelectedIndex < 0)
                        return Pages[0].PageConfig.Title;

                    return Pages[SelectedIndex].PageConfig.Title;
                }
                else
                {
                    return string.Empty;
                }
            }
            //set
            //{
            //    if (mHeading != value)
            //    {
            //        mHeading = value;

            //        NotifyPropertyChanged("SubTitle");
            //    }
            //}
        }

        public IWizardPage ProgressPage
        {
            get
            {
                if (_progressPage == null)
                    _progressPage = new DefaultProgressView();

                return _progressPage;
            }
            set 
            { 
                if (_progressPage != null && _progressPage != value)
                {
                    ReplacePage(_progressPage, value);
                }

                _progressPage = value; 
                NotifyPropertyChanged(nameof(ProgressPage)); 
            }
        }

        public IWizardPage CompletePage
        {
            get
            {
                if (_completePage == null)
                    _completePage = new DefaultCompleteView();

                return _completePage;
            }
            set 
            {
                if (_completePage != null && _completePage != value)
                {
                    ReplacePage(_completePage, value);
                }

                _completePage = value; 
                NotifyPropertyChanged(nameof(CompletePage)); 
            }
        }

        public IWizardPage ErrorPage
        {
            get
            {
                if (_errorPage == null)
                    _errorPage = new DefaultErrorView();

                return _errorPage;
            }
            set 
            {
                if (_errorPage != null && _errorPage != value)
                {
                    ReplacePage(_errorPage, value);
                }

                _errorPage = value; 
                NotifyPropertyChanged(nameof(ErrorPage));
            }
        }

        #region Visibility

        
        public Visibility CompleteButtonVisibility
        {
            get
            {

                if (_buttonVisibility.ContainsKey(WizardButtons.Complete))
                    return _buttonVisibility[WizardButtons.Complete];

                if (!CompleteEnabled)
                    return Visibility.Collapsed;

                return Visibility.Visible;
            }

        }

        public Visibility CancelButtonVisibility
        {
            get
            {
                if (_buttonVisibility.ContainsKey(WizardButtons.Cancel))
                    return _buttonVisibility[WizardButtons.Cancel];

                if (!CancelEnabled)
                    return Visibility.Collapsed;

                return Visibility.Visible;
            }

        }

        public Visibility ProcessButtonVisibility
        {
            get
            {
                if (!ProcessEnabled)
                    return Visibility.Collapsed;

                if (_buttonVisibility.ContainsKey(WizardButtons.Process))
                    return _buttonVisibility[WizardButtons.Process];

                return Visibility.Visible;
            }
           
        }

        public Visibility ButtonStackVisibility
        {
            get
            {
                if (_buttonVisibility.ContainsKey(WizardButtons.All))
                    return _buttonVisibility[WizardButtons.All];

                if (SelectedPage != null)
                {
                    if (SelectedPage.PageConfig?.HideButtons == true)
                        return Visibility.Hidden;
                }
                return Visibility.Visible;
            }
        }

        public Visibility NextButtonVisibility
        {
            get 
            {
                if (!NextEnabled)
                    return Visibility.Collapsed;

                if (_buttonVisibility.ContainsKey(WizardButtons.Next))
                    return _buttonVisibility[WizardButtons.Next];

                return Visibility.Visible;
            }

        }

        public Visibility PreviousButtonVisibility
        {
            get 
            {
                if (!PreviousEnabled)
                    return Visibility.Collapsed;

                if (_buttonVisibility.ContainsKey(WizardButtons.Previous))
                    return _buttonVisibility[WizardButtons.Previous];

                return Visibility.Visible;

            }

        }

        #endregion

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

            PreviousCommand = new DelegateCommand(() =>
            {
                Navigate(NavigationDirection.Backwards);

                //if (SelectedIndex == Pages.IndexOf(ErrorPage))
                //{
                //    SetPage(LastActivePageIndex);
                //}
                //else
                //{
                //    var cuItem = this.Pages[SelectedIndex];

                //    if (CanNavigate(NavigationDirection.Backwards, cuItem))
                //        SetPage(GetPreviousPageIndex(SelectedIndex));
                //}
                

            });

            NextCommand = new DelegateCommand(() =>
            {
                Navigate(NavigationDirection.Forward);
            });

            ProcessButtonCommand = new DelegateCommand(async () => 
            {
                var cuItem = this.Pages[SelectedIndex];

                if (await cuItem.ValidateAsync())
                {
                    if (CanNavigate(NavigationDirection.Forward, cuItem))
                        IsBusy = true;

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

        private bool CanNavigate(NavigationDirection direction, IWizardPage curItem)
        {
            if (curItem.PageConfig.NavigationHandler != null)
            {
                var evt = new WizardNavigationEventArgs()
                {
                    Direction = direction,
                };

                curItem.PageConfig.NavigationHandler(evt);

                return !evt.Handled;
            }

            return true;

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

            _buttonVisibility.Clear();

            this.SelectedPage.PageConfig.OnPageShownHandler?.Invoke(WizardControl);

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
            NotifyPropertyChanged(nameof(SubTitle));
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
            NotifyPropertyChanged(nameof(ButtonStackVisibility));
        }

        /// <summary>
        /// Set the Parameters property of the Pages added to be that of this class. ie centralise them to point here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mPages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RecalculateNavigation();
        }

        private void ReplacePage(IWizardPage oldPage, IWizardPage newPage)
        {
            
            if (Pages.Contains(oldPage))
            {
                if (newPage != null)
                {
                    Pages.Insert(Pages.IndexOf(oldPage), newPage);
                }
                
                Pages.Remove(oldPage);
            }
            else
            {
                if (newPage != null)
                    Pages.Add(newPage);
            }

            
        }

        internal async void Navigate(NavigationDirection direction)
        {
            switch (direction)
            {
                case NavigationDirection.Backwards:
                    {
                        if (SelectedIndex == Pages.IndexOf(ErrorPage))
                        {
                            SetPage(LastActivePageIndex);
                        }
                        else
                        {
                            var cuItem = this.Pages[SelectedIndex];

                            if (CanNavigate(NavigationDirection.Backwards, cuItem))
                                SetPage(GetPreviousPageIndex(SelectedIndex));
                        }
                    }
                    break;
                case NavigationDirection.Forward:
                    {
                        var cuItem = this.Pages[SelectedIndex];

                        if (await cuItem.ValidateAsync())
                        {
                            if (CanNavigate(NavigationDirection.Forward, cuItem))
                                SetPage(GetNextPageIndex(SelectedIndex));
                        }
                    }
                    break;
            }
        }

        internal void UpdateButtonVisibility(WizardButtonVisibility visibility, params WizardButtons[] buttons)
        {
            if (buttons == null || buttons.Length == 0)
            {
                _buttonVisibility.Clear();
            }
            else
            {
                //set the default hidden mode to collapsed(so that the buttons bunch up)
                var realVisibilty = (Visibility)visibility;

                //create a local copy of the buttons array
                var localButtons = new List<WizardButtons>(buttons);

                //if the buttons array conatains WizardButtons.All, rebuild the localbuttons variable all the buttons.  TODO:// This could be update to loop through the Enum instead of being hardcoded
                if (buttons.Contains(WizardButtons.All))
                {
                    localButtons = new List<WizardButtons>() { WizardButtons.Cancel, WizardButtons.Complete, WizardButtons.Next, WizardButtons.Previous, WizardButtons.Process };
                }

                //loop through all the buttons
                foreach (var button in localButtons)
                {
                    //if all buttons use hide not collapsed when hiding all buttons
                    if (_buttonVisibility.ContainsKey(button))
                    {
                        _buttonVisibility[button] = realVisibilty;
                    }
                    else
                    {
                        _buttonVisibility.Add(button, realVisibilty);
                    }
                }
            }

            RecalculateNavigation();
        }

        internal void UpdateStage(WizardStage stage)
        {
            _currentStage = stage;

            RecalculateNavigation();
        }
        #endregion
    }
}
