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
        public readonly static DependencyProperty CancelCommandProperty = DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(WizardControl));
        

        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
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



        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var somePages = Pages;

            _viewModel.Title = Title;
            _viewModel.Pages = Pages;
            _viewModel.CancelCommand = CancelCommand;

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
            }
        }
    }
}
