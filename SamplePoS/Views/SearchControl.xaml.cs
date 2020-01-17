using SamplePoS.ViewModels;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SamplePoS.Views
{
    public sealed partial class SearchControl : UserControl
    {
        private SearchControlViewModel ViewModel => DataContext as SearchControlViewModel;

        public SearchControl()
        {
            this.InitializeComponent();
        }
    }
}
