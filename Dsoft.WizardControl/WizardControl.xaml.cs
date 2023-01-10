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

		/// <summary>
		/// The title property
		/// </summary>
		public readonly static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(WizardControl), new PropertyMetadata(null, OnTitleChanged));
		/// <summary>
		/// The header template property
		/// </summary>
		public readonly static DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(WizardControl), new PropertyMetadata(null, OnHeaderTemplateChanged));
		/// <summary>
		/// The button style property
		/// </summary>
		public readonly static DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(WizardControl), new PropertyMetadata(null, OnButtonStyleChanged));
		/// <summary>
		/// The process mode property
		/// </summary>
		public readonly static DependencyProperty ProcessModeProperty = DependencyProperty.Register("ProcessMode", typeof(ProcessMode), typeof(WizardControl), new PropertyMetadata(ProcessMode.Default, OnProcessModeChanged));

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:TitleChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.Title = (string)e.NewValue;
        }

		/// <summary>
		/// Gets or sets the header template.
		/// </summary>
		/// <value>The header template.</value>
		public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:HeaderTemplateChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh.HeaderTemplate = (DataTemplate)e.NewValue;
        }

		/// <summary>
		/// Gets or sets the button style.
		/// </summary>
		/// <value>The button style.</value>
		public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:ButtonStyleChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
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

		/// <summary>
		/// Gets or sets the process mode.
		/// </summary>
		/// <value>The process mode.</value>
		public ProcessMode ProcessMode
        {
            get { return (ProcessMode)GetValue(ProcessModeProperty); }
            set { SetValue(TitleProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:ProcessModeChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnProcessModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;

            sh._viewModel.ProcessMode = (ProcessMode)e.NewValue;
        }
		#endregion

		#region Pages
		/// <summary>
		/// The items property
		/// </summary>
		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Pages", typeof(ObservableCollection<IWizardPage>), typeof(WizardControl), new PropertyMetadata(new ObservableCollection<IWizardPage>(), OnPagesChanged));
		/// <summary>
		/// The processing page property
		/// </summary>
		public static readonly DependencyProperty ProcessingPageProperty = DependencyProperty.Register("ProcessingPage", typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(null, OnProcessingPageChanged));
		/// <summary>
		/// The complete page property
		/// </summary>
		public static readonly DependencyProperty CompletePageProperty = DependencyProperty.Register("CompletePage", typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(null, OnCompletePageChanged));
		/// <summary>
		/// The error page property
		/// </summary>
		public static readonly DependencyProperty ErrorPageProperty = DependencyProperty.Register("ErrorPage", typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(null, OnErrorPageChanged));

		/// <summary>
		/// Gets or sets the pages.
		/// </summary>
		/// <value>The pages.</value>
		public ObservableCollection<IWizardPage> Pages
        {
            get { return (ObservableCollection<IWizardPage>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:PagesChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnPagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.Pages = (ObservableCollection<IWizardPage>)e.NewValue;
        }

		/// <summary>
		/// Gets or sets the processing page.
		/// </summary>
		/// <value>The processing page.</value>
		public IWizardPage ProcessingPage
        {
            get { return (IWizardPage)GetValue(ProcessingPageProperty); }
            set { SetValue(ProcessingPageProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:ProcessingPageChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnProcessingPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.ProgressPage = (IWizardPage)e.NewValue;
        }

		/// <summary>
		/// Gets or sets the complete page.
		/// </summary>
		/// <value>The complete page.</value>
		public IWizardPage CompletePage
        {
            get { return (IWizardPage)GetValue(CompletePageProperty); }
            set { SetValue(CompletePageProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:CompletePageChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnCompletePageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.CompletePage = (IWizardPage)e.NewValue;
        }

		/// <summary>
		/// Gets or sets the error page.
		/// </summary>
		/// <value>The error page.</value>
		public IWizardPage ErrorPage
        {
            get { return (IWizardPage)GetValue(ErrorPageProperty); }
            set { SetValue(ErrorPageProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:ErrorPageChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnErrorPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.ErrorPage = (IWizardPage)e.NewValue;
        }

		#endregion

		#region Button Titles

		/// <summary>
		/// The process button title property
		/// </summary>
		public readonly static DependencyProperty ProcessButtonTitleProperty = DependencyProperty.Register("ProcessButtonTitle", typeof(string), typeof(WizardControl), new PropertyMetadata("Process", OnProcessButtonTitleChanged));
		/// <summary>
		/// The close button title property
		/// </summary>
		public readonly static DependencyProperty CloseButtonTitleProperty = DependencyProperty.Register("CloseButtonTitle", typeof(string), typeof(WizardControl), new PropertyMetadata("Close", OnCloseButtonTitleChanged));
		/// <summary>
		/// The cancel button title property
		/// </summary>
		public readonly static DependencyProperty CancelButtonTitleProperty = DependencyProperty.Register("CancelButtonTitle", typeof(string), typeof(WizardControl), new PropertyMetadata("Cancel", OnCancelButtonTitleChanged));
		/// <summary>
		/// The next button title property
		/// </summary>
		public readonly static DependencyProperty NextButtonTitleProperty = DependencyProperty.Register("NextButtonTitle", typeof(string), typeof(WizardControl), new PropertyMetadata("Next", OnNextButtonTitleChanged));
		/// <summary>
		/// The previous button title property
		/// </summary>
		public readonly static DependencyProperty PreviousButtonTitleProperty = DependencyProperty.Register("PreviousButtonTitle", typeof(string), typeof(WizardControl), new PropertyMetadata("Previous", OnPreviousButtonTitleChanged));


		/// <summary>
		/// Gets or sets the process button title.
		/// </summary>
		/// <value>The process button title.</value>
		public string ProcessButtonTitle
        {
            get { return (string)GetValue(ProcessButtonTitleProperty); }
            set { SetValue(ProcessButtonTitleProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:ProcessButtonTitleChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnProcessButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.ProcessButtonTitle = (string)e.NewValue;
        }

		/// <summary>
		/// Gets or sets the close button title.
		/// </summary>
		/// <value>The close button title.</value>
		public string CloseButtonTitle
        {
            get => (string)GetValue(CloseButtonTitleProperty);
            set => SetValue(CloseButtonTitleProperty, value);
        }

		/// <summary>
		/// Handles the <see cref="E:CloseButtonTitleChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnCloseButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.CloseButtonTitle = (string)e.NewValue;
        }

		/// <summary>
		/// Gets or sets the cancel button title.
		/// </summary>
		/// <value>The cancel button title.</value>
		public string CancelButtonTitle
        {
            get => (string)GetValue(CancelButtonTitleProperty);
            set => SetValue(CancelButtonTitleProperty, value);
        }

		/// <summary>
		/// Handles the <see cref="E:CancelButtonTitleChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnCancelButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.CancelButtonTitle = (string)e.NewValue;
        }

		/// <summary>
		/// Gets or sets the next button title.
		/// </summary>
		/// <value>The next button title.</value>
		public string NextButtonTitle
        {
            get { return (string)GetValue(NextButtonTitleProperty); }
            set { SetValue(NextButtonTitleProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:NextButtonTitleChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnNextButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.NextButtonTitle = (string)e.NewValue;
        }

		/// <summary>
		/// Gets or sets the previous button title.
		/// </summary>
		/// <value>The previous button title.</value>
		public string PreviousButtonTitle
        {
            get { return (string)GetValue(PreviousButtonTitleProperty); }
            set { SetValue(PreviousButtonTitleProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:PreviousButtonTitleChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnPreviousButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.PreviousButtonTitle = (string)e.NewValue;
        }
		#endregion

		#region Functions

		/// <summary>
		/// The process function property
		/// </summary>
		public static readonly DependencyProperty ProcessFunctionProperty = DependencyProperty.Register("ProcessFunction", typeof(Func<Task<WizardProcessResult>>), typeof(WizardControl), new PropertyMetadata(null, OnProcessFunctionChanged));
		/// <summary>
		/// The close function property
		/// </summary>
		public static readonly DependencyProperty CloseFunctionProperty = DependencyProperty.Register("CloseFunction", typeof(Action), typeof(WizardControl), new PropertyMetadata(null, OnCloseFunctionChanged));
		/// <summary>
		/// The cancel function property
		/// </summary>
		public static readonly DependencyProperty CancelFunctionProperty = DependencyProperty.Register("CancelFunction", typeof(Action), typeof(WizardControl), new PropertyMetadata(null, OnCancelFunctionChanged));

		/// <summary>
		/// Gets or sets the process function.
		/// </summary>
		/// <value>The process function.</value>
		public Func<Task<WizardProcessResult>> ProcessFunction
        {
            get { return (Func<Task<WizardProcessResult>>)GetValue(ProcessFunctionProperty); }
            set { SetValue(ProcessFunctionProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:ProcessFunctionChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnProcessFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.ProcessFunction = (Func<Task<WizardProcessResult>>)e.NewValue;
        }

		/// <summary>
		/// Gets or sets the close function.
		/// </summary>
		/// <value>The close function.</value>
		public Action CloseFunction
        {
            get { return (Action)GetValue(CloseFunctionProperty); }
            set { SetValue(CloseFunctionProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:CloseFunctionChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnCloseFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.CloseFunction = (Action)e.NewValue;
        }

		/// <summary>
		/// Gets or sets the cancel function.
		/// </summary>
		/// <value>The cancel function.</value>
		public Action CancelFunction
        {
            get { return (Action)GetValue(CancelFunctionProperty); }
            set { SetValue(CancelFunctionProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:CancelFunctionChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnCancelFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            sh._viewModel.CancelFunction = (Action)e.NewValue;
        }

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="WizardControl"/> class.
		/// </summary>
		public WizardControl()
        {
            InitializeComponent();

            _viewModel = (WizardControlViewModel)rootGrid.DataContext;
            _viewModel.WizardControl = this;

            _viewModel.OnIsBusyChanged += OnIsBusyChanged;
        }

		/// <summary>
		/// Finalizes an instance of the <see cref="WizardControl"/> class.
		/// </summary>
		~WizardControl()
        {
            _viewModel.OnIsBusyChanged -= OnIsBusyChanged;
        }

		/// <summary>
		/// Called when [is busy changed].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="isbusy">if set to <c>true</c> [isbusy].</param>
		private void OnIsBusyChanged(object sender, bool isbusy)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                //pgrProgess.Visibility = (isbusy) ? Visibility.Visible : Visibility.Hidden;

            }));

        }

		/// <summary>
		/// Gets the available pages.
		/// </summary>
		/// <value>The available pages.</value>
		public List<IWizardPage> AvailablePages => Pages?.ToList();

		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
		/// </summary>
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
		/// Navigates the specified direction.
		/// </summary>
		/// <param name="direction">The direction.</param>
		public void Navigate(NavigationDirection direction)
        {
            _viewModel.Navigate(direction);
        }

		/// <summary>
		/// Updates the button visibility.
		/// </summary>
		/// <param name="visibility">The visibility.</param>
		/// <param name="buttons">The buttons.</param>
		public void UpdateButtonVisibility(WizardButtonVisibility visibility, params WizardButtons[] buttons)
        {
            _viewModel.UpdateButtonVisibility(visibility, buttons);
        }

		#endregion

		/// <summary>
		/// Handles the <see cref="E:Loaded" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectedPage?.PageConfig?.OnPageShownHandler?.Invoke(this);
        }

		/// <summary>
		/// Updates the stage.
		/// </summary>
		/// <param name="stage">The stage.</param>
		public void UpdateStage(WizardStage stage)
        {
            _viewModel.UpdateStage(stage);
        }

		/// <summary>
		/// Recalculates the navigation.
		/// </summary>
		public void RecalculateNavigation()
        {
            _viewModel.RecalculateNavigation();
        }
    }
}
