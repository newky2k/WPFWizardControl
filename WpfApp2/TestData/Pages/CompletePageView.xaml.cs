﻿using Dsoft.WizardControl.WPF;
using System;
using System.Collections.Generic;
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

namespace WpfApp2.TestData.Pages
{
    /// <summary>
    /// Interaction logic for CompletePageView.xaml
    /// </summary>
    public partial class CompletePageView : UserControl, IWizardPage
    {

        private SharedViewModel _viewModel;


        public SharedViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; DataContext = _viewModel; }
        }

        public CompletePageView(SharedViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }

        public string Title => "Wizard Complete";

        public List<KeyValuePair<string, object>> Parameters { get => new List<KeyValuePair<string, object>>(); set => Console.WriteLine(""); }

        public bool IsHidden => false;

        public bool Validate()
        {
            return true;
        }
    }
}