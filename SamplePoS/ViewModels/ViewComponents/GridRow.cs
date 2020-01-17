using Prism.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SamplePoS.ViewModels.ViewComponents
{
    public class GridRow<T> where T : class
    {
        public ICommand DeleteCommand { get; private set; }

        public Func<T, Task> ItemDeleteClicked { get; set; }

        public GridRow()
        {
            this.DeleteCommand = new DelegateCommand(HandleDeleteCommand);
        }

        public void HandleDeleteCommand()
        {
            this.ItemDeleteClicked(this as T);
        }
    }
}
