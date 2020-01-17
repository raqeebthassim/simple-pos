using System;

using SamplePoS.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SamplePoS.Views
{
    public sealed partial class BillingPage : Page
    {
        private BillingViewModel ViewModel => DataContext as BillingViewModel;

        public BillingPage()
        {
            InitializeComponent();
        }
    }
}
