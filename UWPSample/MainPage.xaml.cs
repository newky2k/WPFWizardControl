﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainWindowViewModel _viewModel;

        public MainWindowViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; DataContext = _viewModel; }
        }

        public MainPage()
        {
            this.InitializeComponent();

            ViewModel = new MainWindowViewModel(wzrControl);

            ViewModel.OnRequestCloseWindow += OnCloseWindowRequest;
        }

        private void OnCloseWindowRequest(object sender, bool e)
        {
            Application.Current.Exit();
        }
    }
}