using System;

using SamplePoS.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SamplePoS.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
