using System;

using SamplePoS.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SamplePoS.Views
{
    public sealed partial class CustomersPage : Page
    {
        private CustomersViewModel ViewModel => DataContext as CustomersViewModel;

        public CustomersPage()
        {
            InitializeComponent();
        }
    }
}
