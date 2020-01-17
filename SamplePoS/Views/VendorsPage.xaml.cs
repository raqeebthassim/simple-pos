using System;

using SamplePoS.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SamplePoS.Views
{
    public sealed partial class VendorsPage : Page
    {
        private VendorsViewModel ViewModel => DataContext as VendorsViewModel;

        public VendorsPage()
        {
            InitializeComponent();
        }
    }
}
