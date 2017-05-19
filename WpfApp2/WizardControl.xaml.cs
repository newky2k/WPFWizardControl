using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for WizardControl.xaml
    /// </summary>
    public partial class WizardControl : UserControl
    {
   
        private BaseWizardViewModel mViewModel;

        public static readonly DependencyProperty ItemsProperty =  DependencyProperty.Register("Pages", typeof(ObservableCollection<WizardPage>), typeof(WizardControl), new PropertyMetadata(new ObservableCollection<WizardPage>()));
        public readonly static DependencyProperty TitleProperty =  DependencyProperty.Register("Title", typeof(string), typeof(WizardControl));
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

        public ObservableCollection<WizardPage> Pages
        {
            get { return (ObservableCollection<WizardPage>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value);}
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

        /// <summary>
        /// Set the view model.  Also sets the window DataContext.
        /// </summary>
        private BaseWizardViewModel ViewModel
        {
            get
            {
                return mViewModel;
            }
            set
            {
                mViewModel = value;
                this.DataContext = mViewModel;
            }

        }

        public WizardControl()
        {
            InitializeComponent();


            ViewModel = new BaseWizardViewModel();

         
           
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var somePages = Pages;

            ViewModel.Title = Title;
            ViewModel.Pages = Pages;
            ViewModel.CancelCommand = CancelCommand;
            
            if (HeaderTemplate != null)
            {
                wizControl.HeaderTemplate = HeaderTemplate;
            }
            
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
