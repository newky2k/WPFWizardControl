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
    public partial class WizardControl : UserControl
    {
        private WizardControlViewModel _viewModel;

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Pages", typeof(ObservableCollection<IWizardPage>), typeof(WizardControl), new PropertyMetadata(new ObservableCollection<IWizardPage>()));
        public readonly static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(WizardControl));
        public readonly static DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(WizardControl));
        public readonly static DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(WizardControl));

        public static readonly DependencyProperty ProcessingPageProperty = DependencyProperty.Register("ProcessingPage", typeof(IWizardPage), typeof(WizardControl));
        public static readonly DependencyProperty CompletePageProperty = DependencyProperty.Register("CompletePage", typeof(IWizardPage), typeof(WizardControl));
        public static readonly DependencyProperty ErrorPageProperty = DependencyProperty.Register("ErrorPage", typeof(IWizardPage), typeof(WizardControl));

        public static readonly DependencyProperty ProcessFunctionProperty = DependencyProperty.Register("ProcessFunction", typeof(Func<Task<WizardProcessResult>>), typeof(WizardControl));
        public static readonly DependencyProperty CloseFunctionProperty = DependencyProperty.Register("CloseFunction", typeof(Action), typeof(WizardControl));
        public static readonly DependencyProperty CancelFunctionProperty = DependencyProperty.Register("CancelFunction", typeof(Action), typeof(WizardControl));

        public Func<Task<WizardProcessResult>> ProcessFunction
        {
            get { return (Func<Task<WizardProcessResult>>)GetValue(ProcessFunctionProperty); }
            set { SetValue(ProcessFunctionProperty, value); }
        }

        public Action CloseFunction
        {
            get { return (Action)GetValue(CloseFunctionProperty); }
            set { SetValue(CloseFunctionProperty, value); }
        }

        public Action CancelFunction
        {
            get { return (Action)GetValue(CancelFunctionProperty); }
            set { SetValue(CancelFunctionProperty, value); }
        }


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public ObservableCollection<IWizardPage> Pages
        {
            get { return (ObservableCollection<IWizardPage>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public IWizardPage ProcessingPage
        {
            get { return (IWizardPage)GetValue(ProcessingPageProperty); }
            set { SetValue(ProcessingPageProperty, value); }
        }

        public IWizardPage CompletePage
        {
            get { return (IWizardPage)GetValue(CompletePageProperty); }
            set { SetValue(CompletePageProperty, value); }
        }

        public IWizardPage ErrorPage
        {
            get { return (IWizardPage)GetValue(ErrorPageProperty); }
            set { SetValue(ErrorPageProperty, value); }
        }

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }


        public WizardControl()
        {
            InitializeComponent();


            _viewModel = (WizardControlViewModel)rootGrid.DataContext;

            _viewModel.OnIsBusyChanged += OnIsBusyChanged;

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
    }
}
