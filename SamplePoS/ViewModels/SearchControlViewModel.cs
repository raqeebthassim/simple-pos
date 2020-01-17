using Prism.Windows.Mvvm;

namespace SamplePoS.ViewModels
{
    public class SearchControlViewModel: ViewModelBase
    {
        private string searchText;
        public string SearchText
        {
            get
            { return this.searchText; }
            set
            {
                this.searchText = value;
                RaisePropertyChanged("SearchText");
            }
        }
    }
}
