using System;
using System.Collections.Generic;
using System.IO;
using Windows.Foundation;
using DSoft.WizardControl.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

#if UAP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
#endif

namespace DSoft.WizardControl
{
    public class WizardControl : Control
    {
        #region Dependecy Properties

        #endregion
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(object), typeof(WizardControl), new PropertyMetadata(null, OnHeaderChanged));
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(WizardControl), new PropertyMetadata(null));

        /// <summary>
        ///     Gets or sets the spacing.
        /// </summary>
        /// <value>
        ///     The spacing.
        /// </value>
        public object Header
        {
            get
            {
                return this.GetValue(HeaderProperty);
            }
            set
            {
                this.SetValue(HeaderProperty, value);
            }
        }

        public int SelectedIndex
        {
            get
            {
                return (int)this.GetValue(SelectedIndexProperty);
            }
            set
            {
                this.SetValue(SelectedIndexProperty, value);
            }
        }


        public DataTemplate HeaderTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(HeaderTemplateProperty);
            }
            set
            {
                this.SetValue(HeaderTemplateProperty, value);
            }
        }

        #region Pages
        public static readonly DependencyProperty PagesProperty = DependencyProperty.Register("Pages", typeof(ObservableCollection<IWizardPage>), typeof(WizardControl), new PropertyMetadata(new ObservableCollection<IWizardPage>(), OnPagesChanged));
        public static readonly DependencyProperty ProcessingPageProperty = DependencyProperty.Register("ProcessingPage", typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(null, OnProcessingPageChanged));
        public static readonly DependencyProperty CompletePageProperty = DependencyProperty.Register("CompletePage", typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(null, OnCompletePageChanged));
        public static readonly DependencyProperty ErrorPageProperty = DependencyProperty.Register("ErrorPage", typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(null, OnErrorPageChanged));

        public ObservableCollection<IWizardPage> Pages
        {
            get { return (ObservableCollection<IWizardPage>)GetValue(PagesProperty); }
            set { SetValue(PagesProperty, value); }
        }

        private static void OnPagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.Pages = (ObservableCollection<IWizardPage>)e.NewValue;
        }

        public IWizardPage ProcessingPage
        {
            get { return (IWizardPage)GetValue(ProcessingPageProperty); }
            set { SetValue(ProcessingPageProperty, value); }
        }

        private static void OnProcessingPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.ProgressPage = (IWizardPage)e.NewValue;
        }

        public IWizardPage CompletePage
        {
            get { return (IWizardPage)GetValue(CompletePageProperty); }
            set { SetValue(CompletePageProperty, value); }
        }

        private static void OnCompletePageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.CompletePage = (IWizardPage)e.NewValue;
        }

        public IWizardPage ErrorPage
        {
            get { return (IWizardPage)GetValue(ErrorPageProperty); }
            set { SetValue(ErrorPageProperty, value); }
        }

        private static void OnErrorPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.ErrorPage = (IWizardPage)e.NewValue;
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

        #region MyRegion

        public static readonly DependencyProperty PreviousEnabledProperty = DependencyProperty.Register(nameof(PreviousEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty NextEnabledProperty = DependencyProperty.Register(nameof(NextEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty ProcessEnabledProperty = DependencyProperty.Register(nameof(ProcessEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty CancelEnabledProperty = DependencyProperty.Register(nameof(CancelEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty CompleteEnabledProperty = DependencyProperty.Register(nameof(CompleteEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        /// <summary>
        /// Is previous button enabled
        /// </summary>
        public bool PreviousEnabled
        {
            get { return (bool)GetValue(PreviousEnabledProperty); }
            set { SetValue(PreviousEnabledProperty, value); }
        }

        /// <summary>
        /// Is next button enabled
        /// </summary>
        public bool NextEnabled
        {
            get { return (bool)GetValue(NextEnabledProperty); }
            set { SetValue(NextEnabledProperty, value); }
        }

        /// <summary>
        /// Is finish button enabled
        /// </summary>
        public bool ProcessEnabled
        {
            get { return (bool)GetValue(ProcessEnabledProperty); }
            set { SetValue(ProcessEnabledProperty, value); }
        }

        /// <summary>
        /// Is previous button enabled
        /// </summary>
        public bool CancelEnabled
        {
            get { return (bool)GetValue(CancelEnabledProperty); }
            set { SetValue(CancelEnabledProperty, value); }
        }

        public bool CompleteEnabled
        {
            get { return (bool)GetValue(CompleteEnabledProperty); }
            set { SetValue(CompleteEnabledProperty, value); }
        }

        #endregion

        #region Visibility

        public static readonly DependencyProperty CompleteButtonVisibilityProperty = DependencyProperty.Register(nameof(CompleteButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty CancelButtonVisibilityProperty = DependencyProperty.Register(nameof(CancelButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty ProcessButtonVisibilityProperty = DependencyProperty.Register(nameof(ProcessButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty ButtonStackVisibilityProperty = DependencyProperty.Register(nameof(ButtonStackVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty NextButtonVisibilityProperty = DependencyProperty.Register(nameof(NextButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty PreviousButtonVisibilityProperty = DependencyProperty.Register(nameof(PreviousButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));

        public Visibility CompleteButtonVisibility
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

        public Visibility CancelButtonVisibility
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

        public Visibility ProcessButtonVisibility
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

        public Visibility ButtonStackVisibility
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

        public Visibility NextButtonVisibility
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

        public Visibility PreviousButtonVisibility
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


        #region Commands

        public static readonly DependencyProperty PreviousCommandProperty = DependencyProperty.Register(nameof(PreviousCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty NextCommandProperty = DependencyProperty.Register(nameof(NextCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty ProcessButtonCommandProperty = DependencyProperty.Register(nameof(ProcessButtonCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty CompleteCommandProperty = DependencyProperty.Register(nameof(CompleteCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));




        /// <summary>
        /// Gets the previous command.
        /// </summary>
        /// <value>
        /// The previous command.
        /// </value>
        public ICommand PreviousCommand
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
        public ICommand NextCommand
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

        public ICommand ProcessButtonCommand
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
        public ICommand CompleteCommand
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

        public ICommand CancelCommand
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

        #endregion

        public WizardControl()
        {
            this.DefaultStyleKey = typeof(WizardControl);


        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (WizardControl)d;
            //control.SetHeaderVisibility();
            //control.OnHeaderChanged(e.OldValue, e.NewValue);
        }

    }
}
