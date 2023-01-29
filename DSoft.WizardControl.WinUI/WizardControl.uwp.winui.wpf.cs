using System;
using System.Collections.Generic;
using System.IO;
using DSoft.WizardControl.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Linq;

#if WPF
using System.Windows.Controls;
using System.Windows;
#else
using Windows.UI.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#endif

namespace DSoft.WizardControl
{
	/// <summary>
	/// Class WizardControl.
	/// Implements the <see cref="Control" />
	/// Implements the <see cref="IWizardControl" />
	/// </summary>
	/// <seealso cref="Control" />
	/// <seealso cref="IWizardControl" />
	public class WizardControl : Control, IWizardControl
    {
		#region Controls
		private ContentControl _contentGrid;
        private Button _btnNext;
        private Button _btnPrevious;
        private Button _btnCancel;
        private Button _btnFinish;
        private Button _btnComplete;

        #endregion
        private WizardStage _currentStage = WizardStage.Setup;
        private Dictionary<WizardButtons, Visibility> _buttonVisibility = new Dictionary<WizardButtons, Visibility>();

		#region Events

		/// <summary>
		/// Occurs when [on selected page changed].
		/// </summary>
		public event EventHandler<IWizardPage> OnSelectedPageChanged = delegate { };
		/// <summary>
		/// Occurs when [on selected index changed].
		/// </summary>
		public event EventHandler<int> OnSelectedIndexChanged = delegate { };
		#endregion

		#region Titles and Sub-Title


		/// <summary>
		/// The title property
		/// </summary>
		public readonly static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(WizardControl), new PropertyMetadata("Wizard Title", OnTitleChanged));
		/// <summary>
		/// The process mode property
		/// </summary>
		public readonly static DependencyProperty ProcessModeProperty = DependencyProperty.Register("ProcessMode", typeof(ProcessMode), typeof(WizardControl), new PropertyMetadata(ProcessMode.Default, OnProcessModeChanged));
		/// <summary>
		/// The sub title property
		/// </summary>
		internal readonly static DependencyProperty SubTitleProperty = DependencyProperty.Register(nameof(SubTitle), typeof(string), typeof(WizardControl), new PropertyMetadata("Wizard Sub-Title", OnSubTitleChanged));

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
		/// Gets or sets the sub title.
		/// </summary>
		/// <value>The sub title.</value>
		internal string SubTitle
        {
            get { return (string)GetValue(SubTitleProperty); }
            set { SetValue(SubTitleProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:TitleChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh._viewModel.Title = (string)e.NewValue;
        }

		/// <summary>
		/// Handles the <see cref="E:SubTitleChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnSubTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh._viewModel.Title = (string)e.NewValue;
        }

		/// <summary>
		/// Gets or sets the process mode.
		/// </summary>
		/// <value>The process mode.</value>
		public ProcessMode ProcessMode
        {
            get { return (ProcessMode)GetValue(ProcessModeProperty); }
            set { SetValue(ProcessModeProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:ProcessModeChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnProcessModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;

         //   sh._viewModel.ProcessMode = (ProcessMode)e.NewValue;
        }



		#endregion

		#region Header

		/// <summary>
		/// The title text style property
		/// </summary>
		public static readonly DependencyProperty TitleTextStyleProperty = DependencyProperty.Register(nameof(TitleTextStyle), typeof(Style), typeof(WizardControl), new PropertyMetadata(null));
		/// <summary>
		/// The sub title text style property
		/// </summary>
		public static readonly DependencyProperty SubTitleTextStyleProperty = DependencyProperty.Register(nameof(SubTitleTextStyle), typeof(Style), typeof(WizardControl), new PropertyMetadata(null));

		/// <summary>
		/// The button style property
		/// </summary>
		public readonly static DependencyProperty ButtonStyleProperty = DependencyProperty.Register(nameof(ButtonStyle), typeof(Style), typeof(WizardControl), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the title text style.
		/// </summary>
		/// <value>The title text style.</value>
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

		/// <summary>
		/// Gets or sets the sub title text style.
		/// </summary>
		/// <value>The sub title text style.</value>
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

		/// <summary>
		/// Gets or sets the button style.
		/// </summary>
		/// <value>The button style.</value>
		public Style ButtonStyle
		{
			get { return (Style)GetValue(ButtonStyleProperty); }
			set { SetValue(ButtonStyleProperty, value); }
		}

		///// <summary>
		///// Handles the <see cref="E:ButtonStyleChanged" /> event.
		///// </summary>
		///// <param name="d">The d.</param>
		///// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		//private static void OnButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		//{
		//	WizardControl sh = (WizardControl)d;
		//	//sh.HeaderTemplate = (Style)e.NewValue;

		//	if (sh.ButtonStyle != null)
		//	{
  //              if (sh._btnNext != null)
  //                  sh._btnNext.Style = sh.ButtonStyle;
		//		if (sh._btnCancel != null)
		//			sh._btnCancel.Style = sh.ButtonStyle;
		//		if (sh._btnFinish != null)
		//			sh._btnFinish.Style = sh.ButtonStyle;
		//		if (sh._btnPrevious != null)
		//			sh._btnPrevious.Style = sh.ButtonStyle;
		//		if (sh._btnComplete != null)
		//			sh._btnComplete.Style = sh.ButtonStyle;
		//	}
		//}

		#endregion

		#region Pages
		/// <summary>
		/// The selected index property
		/// </summary>
		public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(WizardControl), new PropertyMetadata(0, OnInternalSelectedIndexChanged));
		/// <summary>
		/// The selected page property
		/// </summary>
		public static readonly DependencyProperty SelectedPageProperty = DependencyProperty.Register(nameof(SelectedPage), typeof(IWizardPage), typeof(WizardControl), new PropertyMetadata(null, OnInternalSelectedPageChanged));
		/// <summary>
		/// The pages property
		/// </summary>
		public static readonly DependencyProperty PagesProperty = DependencyProperty.Register("Pages", typeof(ObservableCollection<IWizardPage>), typeof(WizardControl), new PropertyMetadata(new ObservableCollection<IWizardPage>(), OnPagesChanged));
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
		/// Gets or sets the index of the selected.
		/// </summary>
		/// <value>The index of the selected.</value>
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

		/// <summary>
		/// Handles the <see cref="E:InternalSelectedIndexChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnInternalSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;


        }

		/// <summary>
		/// Gets the last index of the active page.
		/// </summary>
		/// <value>The last index of the active page.</value>
		private int LastActivePageIndex
        {
            get
            {
                return ActivePages.Max(x => x.Key);
            }
        }

		/// <summary>
		/// Gets the active pages.
		/// </summary>
		/// <value>The active pages.</value>
		public Dictionary<int, IWizardPage> ActivePages
        {
            get
            {
                var aDict = new Dictionary<int, IWizardPage>();


                var aPages = Pages.Where(x => x != null && x.PageConfig != null && x.PageConfig.IsHidden.Equals(false) && (!x.Equals(ProcessingPage) && !x.Equals(CompletePage) && !x.Equals(ErrorPage)));

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
		/// Gets or sets the pages.
		/// </summary>
		/// <value>The pages.</value>
		public ObservableCollection<IWizardPage> Pages
        {
            get { return (ObservableCollection<IWizardPage>)GetValue(PagesProperty); }
            set { SetValue(PagesProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:PagesChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnPagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;

            sh.SetPage(0);

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
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnProcessingPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;

            if (e.NewValue != null && sh.ProcessingPage != e.NewValue)
            {
                sh.ReplacePage(sh.ProcessingPage, (IWizardPage)e.NewValue);
            }
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
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnCompletePageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;

            if (e.NewValue != null && sh.CompletePage  != e.NewValue)
            {
                sh.ReplacePage(sh.CompletePage, (IWizardPage)e.NewValue);
            }

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
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnErrorPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;

            if (e.NewValue != null && sh.ErrorPage != e.NewValue)
            {
                sh.ReplacePage(sh.ErrorPage, (IWizardPage)e.NewValue);
            }

        }

		/// <summary>
		/// Gets or sets the selected page.
		/// </summary>
		/// <value>The selected page.</value>
		public IWizardPage SelectedPage
        {
            get { return (IWizardPage)GetValue(SelectedPageProperty); }
            set { SetValue(SelectedPageProperty, value); }
        }

		/// <summary>
		/// Handles the <see cref="E:InternalSelectedPageChanged" /> event.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnInternalSelectedPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;


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
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnProcessButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.ProcessButtonTitle = (string)e.NewValue;
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
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnCloseButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.CloseButtonTitle = (string)e.NewValue;
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
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnCancelButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.CancelButtonTitle = (string)e.NewValue;
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
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnNextButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.NextButtonTitle = (string)e.NewValue;
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
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnPreviousButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sh = (WizardControl)d;
            //sh._viewModel.PreviousButtonTitle = (string)e.NewValue;
        }
        #endregion

        #region Buttons

        internal static readonly DependencyProperty PreviousEnabledProperty = DependencyProperty.Register(nameof(PreviousEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty NextEnabledProperty = DependencyProperty.Register(nameof(NextEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty ProcessEnabledProperty = DependencyProperty.Register(nameof(ProcessEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty CancelEnabledProperty = DependencyProperty.Register(nameof(CancelEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty CompleteEnabledProperty = DependencyProperty.Register(nameof(CompleteEnabled), typeof(bool), typeof(WizardControl), new PropertyMetadata(null));
		/// <summary>
		/// Is previous button enabled
		/// </summary>
		/// <value><c>true</c> if [previous enabled]; otherwise, <c>false</c>.</value>
		internal bool PreviousEnabled
        {
            get { return (bool)GetValue(PreviousEnabledProperty); }
            set { SetValue(PreviousEnabledProperty, value); }
        }

		/// <summary>
		/// Is next button enabled
		/// </summary>
		/// <value><c>true</c> if [next enabled]; otherwise, <c>false</c>.</value>
		internal bool NextEnabled
        {
            get { return (bool)GetValue(NextEnabledProperty); }
            set { SetValue(NextEnabledProperty, value); }
        }

		/// <summary>
		/// Is finish button enabled
		/// </summary>
		/// <value><c>true</c> if [process enabled]; otherwise, <c>false</c>.</value>
		internal bool ProcessEnabled
        {
            get { return (bool)GetValue(ProcessEnabledProperty); }
            set { SetValue(ProcessEnabledProperty, value); }
        }

		/// <summary>
		/// Is previous button enabled
		/// </summary>
		/// <value><c>true</c> if [cancel enabled]; otherwise, <c>false</c>.</value>
		internal bool CancelEnabled
        {
            get { return (bool)GetValue(CancelEnabledProperty); }
            set { SetValue(CancelEnabledProperty, value); }
        }

		/// <summary>
		/// Gets or sets a value indicating whether [complete enabled].
		/// </summary>
		/// <value><c>true</c> if [complete enabled]; otherwise, <c>false</c>.</value>
		internal bool CompleteEnabled
        {
            get { return (bool)GetValue(CompleteEnabledProperty); }
            set { SetValue(CompleteEnabledProperty, value); }
        }

        #endregion

        #region Visibility

        internal static readonly DependencyProperty CompleteButtonVisibilityProperty = DependencyProperty.Register(nameof(CompleteButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty CancelButtonVisibilityProperty = DependencyProperty.Register(nameof(CancelButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty ProcessButtonVisibilityProperty = DependencyProperty.Register(nameof(ProcessButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty ButtonStackVisibilityProperty = DependencyProperty.Register(nameof(ButtonStackVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty NextButtonVisibilityProperty = DependencyProperty.Register(nameof(NextButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty PreviousButtonVisibilityProperty = DependencyProperty.Register(nameof(PreviousButtonVisibility), typeof(Visibility), typeof(WizardControl), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the complete button visibility.
		/// </summary>
		/// <value>The complete button visibility.</value>
		internal Visibility CompleteButtonVisibility
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

		/// <summary>
		/// Gets or sets the cancel button visibility.
		/// </summary>
		/// <value>The cancel button visibility.</value>
		internal Visibility CancelButtonVisibility
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

		/// <summary>
		/// Gets or sets the process button visibility.
		/// </summary>
		/// <value>The process button visibility.</value>
		internal Visibility ProcessButtonVisibility
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

		/// <summary>
		/// Gets or sets the button stack visibility.
		/// </summary>
		/// <value>The button stack visibility.</value>
		internal Visibility ButtonStackVisibility
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

		/// <summary>
		/// Gets or sets the next button visibility.
		/// </summary>
		/// <value>The next button visibility.</value>
		internal Visibility NextButtonVisibility
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

		/// <summary>
		/// Gets or sets the previous button visibility.
		/// </summary>
		/// <value>The previous button visibility.</value>
		internal Visibility PreviousButtonVisibility
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
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnProcessFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh._viewModel.ProcessFunction = (Func<Task<WizardProcessResult>>)e.NewValue;
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
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnCloseFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh._viewModel.CloseFunction = (Action)e.NewValue;
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
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void OnCancelFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WizardControl sh = (WizardControl)d;
            //sh._viewModel.CancelFunction = (Action)e.NewValue;
        }

        #endregion

        #region Commands

        internal static readonly DependencyProperty PreviousCommandProperty = DependencyProperty.Register(nameof(PreviousCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty NextCommandProperty = DependencyProperty.Register(nameof(NextCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty ProcessButtonCommandProperty = DependencyProperty.Register(nameof(ProcessButtonCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty CompleteCommandProperty = DependencyProperty.Register(nameof(CompleteCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));
        internal static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(WizardControl), new PropertyMetadata(null));




		/// <summary>
		/// Gets the previous command.
		/// </summary>
		/// <value>The previous command.</value>
		internal ICommand PreviousCommand
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
		/// <value>The next command.</value>
		internal ICommand NextCommand
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

		/// <summary>
		/// Gets or sets the process button command.
		/// </summary>
		/// <value>The process button command.</value>
		internal ICommand ProcessButtonCommand
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
		/// <value>The complete command.</value>
		internal ICommand CompleteCommand
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

		/// <summary>
		/// Gets or sets the cancel command.
		/// </summary>
		/// <value>The cancel command.</value>
		internal ICommand CancelCommand
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

		/// <summary>
		/// Gets the available pages.
		/// </summary>
		/// <value>The available pages.</value>
		public List<IWizardPage> AvailablePages => Pages?.ToList();

		#endregion

		#region Internal Enabled

		/// <summary>
		/// Is previous button enabled
		/// </summary>
		/// <value><c>true</c> if this instance is previous enabled; otherwise, <c>false</c>.</value>
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
		/// <value><c>true</c> if this instance is next enabled; otherwise, <c>false</c>.</value>
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
		/// <value><c>true</c> if this instance is process enabled; otherwise, <c>false</c>.</value>
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
		/// <value><c>true</c> if this instance is cancel enabled; otherwise, <c>false</c>.</value>
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

		/// <summary>
		/// Gets a value indicating whether this instance is complete enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is complete enabled; otherwise, <c>false</c>.</value>
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


		/// <summary>
		/// Gets the is complete button visibility.
		/// </summary>
		/// <value>The is complete button visibility.</value>
		internal Visibility IsCompleteButtonVisibility
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

		/// <summary>
		/// Gets the is cancel button visibility.
		/// </summary>
		/// <value>The is cancel button visibility.</value>
		internal Visibility IsCancelButtonVisibility
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

		/// <summary>
		/// Gets the is process button visibility.
		/// </summary>
		/// <value>The is process button visibility.</value>
		internal Visibility IsProcessButtonVisibility
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

		/// <summary>
		/// Gets the is button stack visibility.
		/// </summary>
		/// <value>The is button stack visibility.</value>
		internal Visibility IsButtonStackVisibility
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

		/// <summary>
		/// Gets the is next button visibility.
		/// </summary>
		/// <value>The is next button visibility.</value>
		internal Visibility IsNextButtonVisibility
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

		/// <summary>
		/// Gets the is previous button visibility.
		/// </summary>
		/// <value>The is previous button visibility.</value>
		internal Visibility IsPreviousButtonVisibility
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

		/// <summary>
		/// Initializes a new instance of the <see cref="WizardControl" /> class.
		/// </summary>
		public WizardControl()
        {
            this.DefaultStyleKey = typeof(WizardControl);
#if WPF
			DefaultStyleKeyProperty.OverrideMetadata(typeof(WizardControl), new FrameworkPropertyMetadata(typeof(WizardControl)));
#endif
		}
#if WPF
		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
		/// </summary>
		public override void OnApplyTemplate()
#else
		/// <summary>
		/// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
		/// </summary>
		protected override void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();

            var controlGrid = GetTemplateChild("PART_CONTENT");

            _contentGrid = controlGrid as ContentControl;

            //setup buttons
            _btnNext = GetTemplateChild("PART_BTN_NEXT") as Button;
            _btnPrevious = GetTemplateChild("PART_BTN_PREVIOUS") as Button;
            _btnCancel = GetTemplateChild("PART_BTN_CANCEL") as Button;
            _btnComplete = GetTemplateChild("PART_BTN_COMPLETE") as Button;
            _btnFinish = GetTemplateChild("PART_BTN_FINISH") as Button;


			PreviousCommand = new DelegateCommand(() =>
            {
                Navigate(NavigationDirection.Backwards);
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
#if WPF
                    await this.Dispatcher.BeginInvoke((Action)(async () =>
#else
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
#endif

                    {
            try
                        {
                            SetPage(Pages.IndexOf(ProcessingPage));

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
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
#if WPF
                    }));
#else
                    });
#endif
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

            if (Pages != null)
            {
                if (!Pages.Contains(ProcessingPage))
                    Pages.Add(ProcessingPage);

                if (!Pages.Contains(CompletePage))
                    Pages.Add(CompletePage);

                if (!Pages.Contains(ErrorPage))
                    Pages.Add(ErrorPage);
            }

            if (Pages?.Count > 0)
                SelectedPage = Pages[0];

            //sh.RecalculateNavigation();

            SetPage(0);
        }


#region IWizard Elements

		/// <summary>
		/// Starts the processing stage.
		/// </summary>
		public void StartProcessingStage()
        {
            _currentStage = WizardStage.Working;
        }

		/// <summary>
		/// Starts the completion stage.
		/// </summary>
		public void StartCompletionStage()
        {
            _currentStage = WizardStage.Complete;
        }

		/// <summary>
		/// Starts the setup stage.
		/// </summary>
		public void StartSetupStage()
        {
            _currentStage = WizardStage.Setup;
        }

		/// <summary>
		/// Starts the error stage.
		/// </summary>
		public void StartErrorStage()
        {
            _currentStage = WizardStage.Error;
        }

		/// <summary>
		/// Navigates the specified direction.
		/// </summary>
		/// <param name="direction">The direction.</param>
		public async void Navigate(NavigationDirection direction)
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

		/// <summary>
		/// Sets the page.
		/// </summary>
		/// <param name="newIndex">The new index.</param>
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

            if (SelectedPage != null)
            {
                SelectedPage.PageConfig.OnPageShownHandler?.Invoke(this);

                _contentGrid.Content = SelectedPage;
            }

            RecalculateNavigation();
        }

		/// <summary>
		/// Determines whether this instance can navigate the specified direction.
		/// </summary>
		/// <param name="direction">The direction.</param>
		/// <param name="curItem">The current item.</param>
		/// <returns><c>true</c> if this instance can navigate the specified direction; otherwise, <c>false</c>.</returns>
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

		/// <summary>
		/// Updates the button visibility.
		/// </summary>
		/// <param name="visibility">The visibility.</param>
		/// <param name="buttons">The buttons.</param>
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

		/// <summary>
		/// Updates the stage.
		/// </summary>
		/// <param name="stage">The stage.</param>
		public void UpdateStage(WizardStage stage)
        {
            _currentStage = stage;

            RecalculateNavigation();
        }

		/// <summary>
		/// Recalculates the navigation.
		/// </summary>
		public void RecalculateNavigation()
        {
            var subTitle = string.Empty;

            if (Pages != null && Pages.Count != 0)
            {
                IWizardPage page;

                if (SelectedIndex < 0)
                    page = Pages[0];
                else
					page = Pages[SelectedIndex];

                if (page != null)
                {
                    subTitle = page.PageConfig?.Title;
                }
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

		/// <summary>
		/// Gets the index of the previous page.
		/// </summary>
		/// <param name="currentIndex">Index of the current.</param>
		/// <returns>System.Int32.</returns>
		private int GetPreviousPageIndex(int currentIndex)
        {
            if (currentIndex == 0)
                return currentIndex;

            var newIndex = currentIndex - 1;


            if (Pages[newIndex].PageConfig.IsHidden)
                return GetPreviousPageIndex(newIndex);

            return newIndex;
        }

		/// <summary>
		/// Gets the index of the next page.
		/// </summary>
		/// <param name="currentIndex">Index of the current.</param>
		/// <returns>System.Int32.</returns>
		private int GetNextPageIndex(int currentIndex)
        {
            if (currentIndex == ActivePages.Max(x => x.Key))
                return currentIndex;

            var newIndex = currentIndex + 1;

            if (Pages[newIndex].PageConfig.IsHidden)
                return GetNextPageIndex(newIndex);

            return newIndex;
        }

		/// <summary>
		/// Replaces the page.
		/// </summary>
		/// <param name="oldPage">The old page.</param>
		/// <param name="newPage">The new page.</param>
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
