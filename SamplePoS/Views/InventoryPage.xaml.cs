using System;

using SamplePoS.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SamplePoS.Views
{
    public sealed partial class InventoryPage : Page
    {
        private InventoryViewModel ViewModel => DataContext as InventoryViewModel;

        public InventoryPage()
        {
            InitializeComponent();
        }
    }
}
