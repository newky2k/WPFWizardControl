using DSoft.WizardControl.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dsoft.WizardControl.WPF
{
    /// <summary>
    /// Interaction logic for WizardControl.xaml
    /// </summary>
    public partial class WizardControl : UserControl, IWizardControl
    {
        private WizardControlViewModel _viewModel;

        #region Properties

        public readonly static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(WizardControl), new PropertyMetadata(null, OnTitleChanged));
        public readonly static DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(WizardControl), new PropertyMetadata(null, OnHeaderTemplateChanged));
        public readonly static DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(WizardControl), new PropertyMetadata(null, OnButtonStyleChanged));
        public readonly static DependencyProperty ProcessModeProperty = DependencyProperty.Register("ProcessMode", typeof(ProcessMode), typeof(WizardControl), new PropertyMetadata(ProcessMode.Default, OnProcessModeChanged));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.Title = (string)e.NewValue;
        }

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh.HeaderTemplate = (DataTemplate)e.NewValue;
        }

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        private static void OnButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh.HeaderTemplate = (Style)e.NewValue;

            if (sh.ButtonStyle != null)
            {
                sh.btnFinish.Style = sh.ButtonStyle;
                sh.btnCancel.Style = sh.ButtonStyle;
                sh.btnNext.Style = sh.ButtonStyle;
                sh.btnPrevious.Style = sh.ButtonStyle;
                sh.btnComplete.Style = sh.ButtonStyle;
            }
        }

        public ProcessMode ProcessMode
        {
            get { return (ProcessMode)GetValue(ProcessModeProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private static void OnProcessModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;

            sh._viewModel.ProcessMode = (ProcessMode)e.NewValue;
        }
        #endregion

        #region Pages
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Pages", typeof(ObservableCollection<IWizardPage>), typeof(WizardControl), new PropertyMetadata(new ObservableCollection<IWizardPage>(), OnPagesChanged));
        public static readonly DependencyProperty ProcessingPageProperty = DependencyProperty.Register("ProcessingPage", typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(null, OnProcessingPageChanged));
        public static readonly DependencyProperty CompletePageProperty = DependencyProperty.Register("CompletePage", typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(null, OnCompletePageChanged));
        public static readonly DependencyProperty ErrorPageProperty = DependencyProperty.Register("ErrorPage", typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(null, OnErrorPageChanged));

        public ObservableCollection<IWizardPage> Pages
        {
            get { return (ObservableCollection<IWizardPage>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        private static void OnPagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.Pages = (ObservableCollection<IWizardPage>)e.NewValue;
        }

        public IWizardPage ProcessingPage
        {
            get { return (IWizardPage)GetValue(ProcessingPageProperty); }
            set { SetValue(ProcessingPageProperty, value); }
        }

        private static void OnProcessingPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.ProgressPage = (IWizardPage)e.NewValue;
        }

        public IWizardPage CompletePage
        {
            get { return (IWizardPage)GetValue(CompletePageProperty); }
            set { SetValue(CompletePageProperty, value); }
        }

        private static void OnCompletePageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.CompletePage = (IWizardPage)e.NewValue;
        }

        public IWizardPage ErrorPage
        {
            get { return (IWizardPage)GetValue(ErrorPageProperty); }
            set { SetValue(ErrorPageProperty, value); }
        }

        private static void OnErrorPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.ErrorPage = (IWizardPage)e.NewValue;
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
            WizardControl sh = (WizardControl)d;
            sh._viewModel.ProcessButtonTitle = (string)e.NewValue;
        }

        public string CloseButtonTitle
        {
            get => (string)GetValue(CloseButtonTitleProperty);
            set => SetValue(CloseButtonTitleProperty, value);
        }

        private static void OnCloseButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.CloseButtonTitle = (string)e.NewValue;
        }

        public string CancelButtonTitle
        {
            get => (string)GetValue(CancelButtonTitleProperty);
            set => SetValue(CancelButtonTitleProperty, value);
        }

        private static void OnCancelButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.CancelButtonTitle = (string)e.NewValue;
        }

        public string NextButtonTitle
        {
            get { return (string)GetValue(NextButtonTitleProperty); }
            set { SetValue(NextButtonTitleProperty, value); }
        }

        private static void OnNextButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.NextButtonTitle = (string)e.NewValue;
        }

        public string PreviousButtonTitle
        {
            get { return (string)GetValue(PreviousButtonTitleProperty); }
            set { SetValue(PreviousButtonTitleProperty, value); }
        }

        private static void OnPreviousButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.PreviousButtonTitle = (string)e.NewValue;
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
            sh._viewModel.ProcessFunction = (Func<Task<WizardProcessResult>>)e.NewValue;
        }

        public Action CloseFunction
        {
            get { return (Action)GetValue(CloseFunctionProperty); }
            set { SetValue(CloseFunctionProperty, value); }
        }

        private static void OnCloseFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.CloseFunction = (Action)e.NewValue;
        }

        public Action CancelFunction
        {
            get { return (Action)GetValue(CancelFunctionProperty); }
            set { SetValue(CancelFunctionProperty, value); }
        }

        private static void OnCancelFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.CancelFunction = (Action)e.NewValue;
        }

        #endregion

        public WizardControl()
        {
            InitializeComponent();

            _viewModel = (WizardControlViewModel)rootGrid.DataContext;
            _viewModel.WizardControl = this;

            _viewModel.OnIsBusyChanged += OnIsBusyChanged;
        }

        ~WizardControl()
        {
            _viewModel.OnIsBusyChanged -= OnIsBusyChanged;
        }

        private void OnIsBusyChanged(object sender, bool isbusy)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                //pgrProgess.Visibility = (isbusy) ? Visibility.Visible : Visibility.Hidden;

            }));

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var somePages = Pages;

            _viewModel.Title = Title;
            _viewModel.ProcessMode = ProcessMode;
            _viewModel.ProcessButtonTitle = ProcessButtonTitle;
            _viewModel.CloseButtonTitle = CloseButtonTitle;
            _viewModel.CancelButtonTitle = CancelButtonTitle;
            _viewModel.NextButtonTitle = NextButtonTitle;
            _viewModel.PreviousButtonTitle = PreviousButtonTitle;

            _viewModel.CompletePage = CompletePage;
            _viewModel.ErrorPage = ErrorPage;
            _viewModel.ProgressPage = ProcessingPage;

            _viewModel.Pages = Pages;

            _viewModel.ProcessFunction = ProcessFunction;
            _viewModel.CloseFunction = CloseFunction;
            _viewModel.CancelFunction = CancelFunction;

            //if (HeaderTemplate != null)
            //{
            //    wizControl.HeaderTemplate = HeaderTemplate;
            //}

            if (ButtonStyle != null)
            {
                btnFinish.Style = ButtonStyle;
                btnCancel.Style = ButtonStyle;
                btnNext.Style = ButtonStyle;
                btnPrevious.Style = ButtonStyle;
                btnComplete.Style = ButtonStyle;
            }
        }

        #region IWizardControl members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        public void Navigate(NavigationDirection direction)
        {
            _viewModel.Navigate(direction);
        }

        public void SetButtonVisibility(WizardButtonVisibility visibility, params WizardButtons[] buttons)
        {
            _viewModel.UpdateButtonVisibility(visibility, buttons);
        }

        #endregion

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectedPage?.PageConfig?.OnPageShownHandler?.Invoke(this);
        }
    }
}
