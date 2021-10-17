﻿using System;
using System.Collections.Generic;
using System.IO;
using Windows.Foundation;
using DSoft.WizardControl.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Linq;

#if UAP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
#endif

namespace DSoft.WizardControl
{
    public class WizardControl : Control, IWizardControl
    {
        private ContentControl _contentGrid;
        private WizardStage _currentStage = WizardStage.Setup;
        private Dictionary<WizardButtons, Visibility> _buttonVisibility = new Dictionary<WizardButtons, Visibility>();

        #region Events

        public event EventHandler<IWizardPage> OnSelectedPageChanged = delegate { };
        public event EventHandler<int> OnSelectedIndexChanged = delegate { };
        #endregion

        #region Titles and Sub-Title

 
        public readonly static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(WizardControl), new PropertyMetadata("Wizard Title", OnTitleChanged));
        public readonly static DependencyProperty ProcessModeProperty = DependencyProperty.Register("ProcessMode", typeof(ProcessMode), typeof(WizardControl), new PropertyMetadata(ProcessMode.Default, OnProcessModeChanged));
        private readonly static DependencyProperty SubTitleProperty = DependencyProperty.Register(nameof(SubTitle), typeof(string), typeof(WizardControl), new PropertyMetadata("Wizard Sub-Title", OnSubTitleChanged));


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private string SubTitle
        {
            get { return (string)GetValue(SubTitleProperty); }
            set { SetValue(SubTitleProperty, value); }
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh._viewModel.Title = (string)e.NewValue;
        }

        private static void OnSubTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh._viewModel.Title = (string)e.NewValue;
        }

        public ProcessMode ProcessMode
        {
            get { return (ProcessMode)GetValue(ProcessModeProperty); }
            set { SetValue(ProcessModeProperty, value); }
        }

        private static void OnProcessModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;

         //   sh._viewModel.ProcessMode = (ProcessMode)e.NewValue;
        }

        #endregion

        #region Header

        public static readonly DependencyProperty TitleTextStyleProperty = DependencyProperty.Register(nameof(TitleTextStyle), typeof(Style), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty SubTitleTextStyleProperty = DependencyProperty.Register(nameof(SubTitleTextStyle), typeof(Style), typeof(WizardControl), new PropertyMetadata(null));

        public Style TitleTextStyle
        {
            get
            {
                return (Style)this.GetValue(TitleTextStyleProperty);
            }
            set
            {
                this.SetValue(TitleTextStyleProperty, value);
            }
        }

        public Style SubTitleTextStyle
        {
            get
            {
                return (Style)this.GetValue(SubTitleTextStyleProperty);
            }
            set
            {
                this.SetValue(SubTitleTextStyleProperty, value);
            }
        }

        #endregion

        #region Pages
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(WizardControl), new PropertyMetadata(0, OnInternalSelectedIndexChanged));
        public static readonly DependencyProperty SelectedPageProperty = DependencyProperty.Register(nameof(SelectedPage), typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(null, OnInternalSelectedPageChanged));
        public static readonly DependencyProperty PagesProperty = DependencyProperty.Register("Pages", typeof(ObservableCollection<IWizardPage>), typeof(WizardControl), new PropertyMetadata(new ObservableCollection<IWizardPage>(), OnPagesChanged));
        public static readonly DependencyProperty ProcessingPageProperty = DependencyProperty.Register("ProcessingPage", typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(new DefaultProgressView(), OnProcessingPageChanged));
        public static readonly DependencyProperty CompletePageProperty = DependencyProperty.Register("CompletePage", typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(new DefaultCompleteView(), OnCompletePageChanged));
        public static readonly DependencyProperty ErrorPageProperty = DependencyProperty.Register("ErrorPage", typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(new DefaultErrorView(), OnErrorPageChanged));

        public int SelectedIndex
        {
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }

        private static void OnInternalSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;


        }

        private int LastActivePageIndex
        {
            get
            {
                return ActivePages.Max(x => x.Key);
            }
        }

        public Dictionary<int, IWizardPage> ActivePages
        {
            get
            {
                var aDict = new Dictionary<int, IWizardPage>();

                var aPages = Pages.Where(x => x.PageConfig.IsHidden.Equals(false) && (!x.Equals(ProcessingPage) && !x.Equals(CompletePage) && !x.Equals(ErrorPage)));

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

        public ObservableCollection<IWizardPage> Pages
        {
            get { return (ObservableCollection<IWizardPage>)GetValue(PagesProperty); }
            set { SetValue(PagesProperty, value); }
        }

        private static void OnPagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;

            if (sh.Pages != null)
            {
                if (!sh.Pages.Contains(sh.ProcessingPage))
                    sh.Pages.Add(sh.ProcessingPage);

                if (!sh.Pages.Contains(sh.CompletePage))
                    sh.Pages.Add(sh.CompletePage);

                if (!sh.Pages.Contains(sh.ErrorPage))
                    sh.Pages.Add(sh.ErrorPage);
            }

            if (sh.Pages?.Count > 0)
                sh.SelectedPage = sh.Pages[0];

            sh.RecalculateNavigation();
        }

        public IWizardPage ProcessingPage
        {
            get { return (IWizardPage)GetValue(ProcessingPageProperty); }
            set { SetValue(ProcessingPageProperty, value); }
        }

        private static void OnProcessingPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;

            if (e.NewValue != null && sh.ProcessingPage != e.NewValue)
            {
                sh.ReplacePage(sh.ProcessingPage, (IWizardPage)e.NewValue);
            }
        }

        public IWizardPage CompletePage
        {
            get { return (IWizardPage)GetValue(CompletePageProperty); }
            set { SetValue(CompletePageProperty, value); }
        }

        private static void OnCompletePageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;

            if (e.NewValue != null && sh.CompletePage  != e.NewValue)
            {
                sh.ReplacePage(sh.CompletePage, (IWizardPage)e.NewValue);
            }

        }

        public IWizardPage ErrorPage
        {
            get { return (IWizardPage)GetValue(ErrorPageProperty); }
            set { SetValue(ErrorPageProperty, value); }
        }

        private static void OnErrorPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;

            if (e.NewValue != null && sh.ErrorPage != e.NewValue)
            {
                sh.ReplacePage(sh.ErrorPage, (IWizardPage)e.NewValue);
            }

        }

        public IWizardPage SelectedPage
        {
            get { return (IWizardPage)GetValue(SelectedPageProperty); }
            set { SetValue(SelectedPageProperty, value); }
        }

        private static void OnInternalSelectedPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;


        }
        #endregion

        #region Button Titles

        public readonly static DependencyProperty ProcessButtonTitleProperty = DependencyProperty.Register("ProcessButtonTitle", typeof(string), typeof(WizardControl), new PropertyMetadata("Process", OnProcessButtonTitleChanged));
        public readonly static DependencyProperty CloseButtonTitleProperty = DependencyProperty.Register("CloseButtonTitle", typeof(string), typeof(WizardControl), new PropertyMetadata("Close", OnCloseButtonTitleChanged));
        public readonly static DependencyProperty CancelButtonTitleProperty = DependencyProperty.Register("CancelButtonTitle", typeof(string), typeof(WizardControl), new PropertyMetadata("Cancel", OnCancelButtonTitleChanged));
        public readonly static DependencyProperty NextButtonTitleProperty = DependencyProperty.Register("NextButtonTitle", typeof(string), typeof(WizardControl), new PropertyMetadata("Next", OnNextButtonTitleChanged));
        public readonly static DependencyProperty PreviousButtonTitleProperty = DependencyProperty.Register("PreviousButtonTitle", typeof(string), typeof(WizardControl), new PropertyMetadata("Previous", OnPreviousButtonTitleChanged));


        public string ProcessButtonTitle
        {
            get { return (string)GetValue(ProcessButtonTitleProperty); }
            set { SetValue(ProcessButtonTitleProperty, value); }
        }

        private static void OnProcessButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.ProcessButtonTitle = (string)e.NewValue;
        }

        public string CloseButtonTitle
        {
            get => (string)GetValue(CloseButtonTitleProperty);
            set => SetValue(CloseButtonTitleProperty, value);
        }

        private static void OnCloseButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.CloseButtonTitle = (string)e.NewValue;
        }

        public string CancelButtonTitle
        {
            get => (string)GetValue(CancelButtonTitleProperty);
            set => SetValue(CancelButtonTitleProperty, value);
        }

        private static void OnCancelButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.CancelButtonTitle = (string)e.NewValue;
        }

        public string NextButtonTitle
        {
            get { return (string)GetValue(NextButtonTitleProperty); }
            set { SetValue(NextButtonTitleProperty, value); }
        }

        private static void OnNextButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.NextButtonTitle = (string)e.NewValue;
        }

        public string PreviousButtonTitle
        {
            get { return (string)GetValue(PreviousButtonTitleProperty); }
            set { SetValue(PreviousButtonTitleProperty, value); }
        }

        private static void OnPreviousButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.PreviousButtonTitle = (string)e.NewValue;
        }
        #endregion

        #region Buttons

        private static readonly DependencyProperty PreviousEnabledProperty = DependencyProperty.Register(nameof(PreviousEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty NextEnabledProperty = DependencyProperty.Register(nameof(NextEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty ProcessEnabledProperty = DependencyProperty.Register(nameof(ProcessEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty CancelEnabledProperty = DependencyProperty.Register(nameof(CancelEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty CompleteEnabledProperty = DependencyProperty.Register(nameof(CompleteEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        /// <summary>
        /// Is previous button enabled
        /// </summary>
        private bool PreviousEnabled
        {
            get { return (bool)GetValue(PreviousEnabledProperty); }
            set { SetValue(PreviousEnabledProperty, value); }
        }

        /// <summary>
        /// Is next button enabled
        /// </summary>
        private bool NextEnabled
        {
            get { return (bool)GetValue(NextEnabledProperty); }
            set { SetValue(NextEnabledProperty, value); }
        }

        /// <summary>
        /// Is finish button enabled
        /// </summary>
        private bool ProcessEnabled
        {
            get { return (bool)GetValue(ProcessEnabledProperty); }
            set { SetValue(ProcessEnabledProperty, value); }
        }

        /// <summary>
        /// Is previous button enabled
        /// </summary>
        private bool CancelEnabled
        {
            get { return (bool)GetValue(CancelEnabledProperty); }
            set { SetValue(CancelEnabledProperty, value); }
        }

        private bool CompleteEnabled
        {
            get { return (bool)GetValue(CompleteEnabledProperty); }
            set { SetValue(CompleteEnabledProperty, value); }
        }

        #endregion

        #region Visibility

        private static readonly DependencyProperty CompleteButtonVisibilityProperty = DependencyProperty.Register(nameof(CompleteButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty CancelButtonVisibilityProperty = DependencyProperty.Register(nameof(CancelButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty ProcessButtonVisibilityProperty = DependencyProperty.Register(nameof(ProcessButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty ButtonStackVisibilityProperty = DependencyProperty.Register(nameof(ButtonStackVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty NextButtonVisibilityProperty = DependencyProperty.Register(nameof(NextButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty PreviousButtonVisibilityProperty = DependencyProperty.Register(nameof(PreviousButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));

        private Visibility CompleteButtonVisibility
        {
            get
            {
                return (Visibility)this.GetValue(CompleteButtonVisibilityProperty);
            }
            set
            {
                this.SetValue(CompleteButtonVisibilityProperty, value);
            }

        }

        private Visibility CancelButtonVisibility
        {
            get
            {
                return (Visibility)this.GetValue(CancelButtonVisibilityProperty);
            }
            set
            {
                this.SetValue(CancelButtonVisibilityProperty, value);
            }

        }

        private Visibility ProcessButtonVisibility
        {
            get
            {
                return (Visibility)this.GetValue(ProcessButtonVisibilityProperty);
            }
            set
            {
                this.SetValue(ProcessButtonVisibilityProperty, value);
            }

        }

        private Visibility ButtonStackVisibility
        {
            get
            {
                return (Visibility)this.GetValue(ButtonStackVisibilityProperty);
            }
            set
            {
                this.SetValue(ButtonStackVisibilityProperty, value);
            }
        }

        private Visibility NextButtonVisibility
        {
            get
            {
                return (Visibility)this.GetValue(NextButtonVisibilityProperty);
            }
            set
            {
                this.SetValue(NextButtonVisibilityProperty, value);
            }

        }

        private Visibility PreviousButtonVisibility
        {
            get
            {
                return (Visibility)this.GetValue(PreviousButtonVisibilityProperty);
            }
            set
            {
                this.SetValue(PreviousButtonVisibilityProperty, value);
            }

        }

        #endregion


        #region Functions

        public static readonly DependencyProperty ProcessFunctionProperty = DependencyProperty.Register("ProcessFunction", typeof(Func<Task<WizardProcessResult>>), typeof(WizardControl), new PropertyMetadata(null, OnProcessFunctionChanged));
        public static readonly DependencyProperty CloseFunctionProperty = DependencyProperty.Register("CloseFunction", typeof(Action), typeof(WizardControl), new PropertyMetadata(null, OnCloseFunctionChanged));
        public static readonly DependencyProperty CancelFunctionProperty = DependencyProperty.Register("CancelFunction", typeof(Action), typeof(WizardControl), new PropertyMetadata(null, OnCancelFunctionChanged));

        public Func<Task<WizardProcessResult>> ProcessFunction
        {
            get { return (Func<Task<WizardProcessResult>>)GetValue(ProcessFunctionProperty); }
            set { SetValue(ProcessFunctionProperty, value); }
        }

        private static void OnProcessFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh._viewModel.ProcessFunction = (Func<Task<WizardProcessResult>>)e.NewValue;
        }

        public Action CloseFunction
        {
            get { return (Action)GetValue(CloseFunctionProperty); }
            set { SetValue(CloseFunctionProperty, value); }
        }

        private static void OnCloseFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh._viewModel.CloseFunction = (Action)e.NewValue;
        }

        public Action CancelFunction
        {
            get { return (Action)GetValue(CancelFunctionProperty); }
            set { SetValue(CancelFunctionProperty, value); }
        }

        private static void OnCancelFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh._viewModel.CancelFunction = (Action)e.NewValue;
        }

        #endregion

        #region Commands

        private static readonly DependencyProperty PreviousCommandProperty = DependencyProperty.Register(nameof(PreviousCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty NextCommandProperty = DependencyProperty.Register(nameof(NextCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty ProcessButtonCommandProperty = DependencyProperty.Register(nameof(ProcessButtonCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty CompleteCommandProperty = DependencyProperty.Register(nameof(CompleteCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));
        private static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));




        /// <summary>
        /// Gets the previous command.
        /// </summary>
        /// <value>
        /// The previous command.
        /// </value>
        private ICommand PreviousCommand
        {
            get
            {
                return (ICommand)this.GetValue(PreviousCommandProperty);
            }
            set
            {
                this.SetValue(PreviousCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets the next command.
        /// </summary>
        /// <value>
        /// The next command.
        /// </value>
        private ICommand NextCommand
        {
            get
            {
                return (ICommand)this.GetValue(NextCommandProperty);
            }
            set
            {
                this.SetValue(NextCommandProperty, value);
            }
        }

        private ICommand ProcessButtonCommand
        {
            get
            {
                return (ICommand)this.GetValue(ProcessButtonCommandProperty);
            }
            set
            {
                this.SetValue(ProcessButtonCommandProperty, value);
            }
        }

        /// <summary>
        /// Command to be called when the wizard completes.  Should be set in the View to be called by the view model
        /// </summary>
        private ICommand CompleteCommand
        {
            get
            {
                return (ICommand)this.GetValue(CompleteCommandProperty);
            }
            set
            {
                this.SetValue(CompleteCommandProperty, value);
            }
        }

        private ICommand CancelCommand
        {
            get
            {
                return (ICommand)this.GetValue(CancelCommandProperty);
            }
            set
            {
                this.SetValue(CancelCommandProperty, value);
            }
        }

        public List<IWizardPage> AvailablePages => Pages?.ToList();

        #endregion

        #region Internal Enabled

        /// <summary>
        /// Is previous button enabled
        /// </summary>
        private bool IsPreviousEnabled
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
        private bool IsNextEnabled
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
        private bool IsProcessEnabled
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
        private bool IsCancelEnabled
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

        private bool IsCompleteEnabled
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


        #endregion


        #region Visibility


        private Visibility IsCompleteButtonVisibility
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

        private Visibility IsCancelButtonVisibility
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

        private Visibility IsProcessButtonVisibility
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

        private Visibility IsButtonStackVisibility
        {
            get
            {
                if (_buttonVisibility.ContainsKey(WizardButtons.All))
                    return _buttonVisibility[WizardButtons.All];

                if (SelectedPage != null)
                {
                    if (SelectedPage.PageConfig?.HideButtons == true)
                        return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        private Visibility IsNextButtonVisibility
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

        private Visibility IsPreviousButtonVisibility
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

        public WizardControl()
        {
            this.DefaultStyleKey = typeof(WizardControl);

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var controlGrid = GetTemplateChild("PART_CONTENT");

            _contentGrid = controlGrid as ContentControl;

            PreviousCommand = new DelegateCommand(() =>
            {
                Navigate(NavigationDirection.Backwards);
            });

            NextCommand = new DelegateCommand(() =>
            {
                Navigate(NavigationDirection.Forward);
            });

            ProcessButtonCommand = new DelegateCommand(() =>
            {
                var cuItem = this.Pages[SelectedIndex];

                if (cuItem.Validate())
                {
                    //if (CanNavigate(NavigationDirection.Forward, cuItem))
                    //    IsBusy = true;

                    Task.Run(async () =>
                    {
                        SetPage(Pages.IndexOf(ProcessingPage));

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

            SetPage(0);
        }


        #region IWizard Elements

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

        public void Navigate(NavigationDirection direction)
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

                        if (cuItem.Validate())
                        {
                            if (CanNavigate(NavigationDirection.Forward, cuItem))
                                SetPage(GetNextPageIndex(SelectedIndex));
                        }
                    }
                    break;
            }
        }

        internal void SetPage(int newIndex)
        {
            if (Pages == null || Pages.Count == 0)
            {
                _contentGrid.Content = ProcessingPage;

                return;
            }
            if (newIndex == Pages.IndexOf(ProcessingPage))
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
            SelectedPage = Pages[SelectedIndex];

            _buttonVisibility.Clear();

            SelectedPage.PageConfig.OnPageShownHandler?.Invoke(this);

            _contentGrid.Content = SelectedPage;

            RecalculateNavigation();
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

        public void UpdateButtonVisibility(WizardButtonVisibility visibility, params WizardButtons[] buttons)
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

        public void UpdateStage(WizardStage stage)
        {
            _currentStage = stage;

            RecalculateNavigation();
        }

        public void RecalculateNavigation()
        {
            var subTitle = string.Empty;

            if (Pages != null && Pages.Count != 0)
            {
                if (SelectedIndex < 0)
                    subTitle = Pages[0].PageConfig.Title;
                else
                    subTitle = Pages[SelectedIndex].PageConfig.Title;
            }

            SubTitle = subTitle;


            //calculate enabled status first
            PreviousEnabled = IsPreviousEnabled;
            NextEnabled = IsNextEnabled;
            ProcessEnabled = IsProcessEnabled;
            CancelEnabled = IsCancelEnabled;
            CompleteEnabled = IsCompleteEnabled;


            //calculate visibility after enabled status
            var buttonStackVisibility = Visibility.Visible;

            if (_buttonVisibility.ContainsKey(WizardButtons.All))
            {
                buttonStackVisibility = _buttonVisibility[WizardButtons.All];
            }
            else if (SelectedPage != null)
            {
                if (SelectedPage.PageConfig?.HideButtons == true)
                    buttonStackVisibility = Visibility.Collapsed;
            }

            ButtonStackVisibility = buttonStackVisibility;

            CompleteButtonVisibility = IsCompleteButtonVisibility;
            CancelButtonVisibility = IsCancelButtonVisibility;
            ProcessButtonVisibility = IsProcessButtonVisibility;
            NextButtonVisibility = IsNextButtonVisibility;
            PreviousButtonVisibility = IsPreviousButtonVisibility;

            SubTitle = SubTitle;
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

        #endregion


    }
}
