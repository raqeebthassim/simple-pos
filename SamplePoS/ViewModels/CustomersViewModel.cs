using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Prism.Commands;
using Prism.Windows.Mvvm;
using SamplePoS.Core.Models;
using SamplePoS.Core.Persistance;
using SamplePoS.ViewModels.ViewComponents;
using Windows.UI.Xaml.Controls;

namespace SamplePoS.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        private readonly ICustomerRepository customerRepository;
        private ObservableCollection<CustomerItemRow> customers;
        private ContentDialog messageDialog;
        private Customer customer;
        public ICommand CellEditEndedCommand { get; private set; }
        public ICommand AddCustomerCommand { get; private set; }
        public ICommand ResetCustomerCommand { get; private set; }



        public CustomersViewModel(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
            this.customers = new ObservableCollection<CustomerItemRow>();
            this.messageDialog = new ContentDialog()
            {
                Title = "Confirm",
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "Cancel",
                Content = "Are you sure you wanna delete?"
            };
            this.CellEditEndedCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(HandleCellEditEnded);
            this.AddCustomerCommand = new DelegateCommand(HandleAddCustomerCommand);
            this.ResetCustomerCommand = new DelegateCommand(HandleResetCustomerCommand);
            this.Customer = new Customer();
            this.populateVendors();
        }

        private void HandleResetCustomerCommand()
        {
            this.Customer = new Customer();
        }

        private void HandleAddCustomerCommand()
        {
            if (this.Customer != null)
            {
                this.customerRepository.Create(this.Customer);
                populateVendors();
                RaisePropertyChanged("Customers");
                HandleResetCustomerCommand();
            }
        }

        private void HandleCellEditEnded(DataGridCellEditEndingEventArgs obj)
        {
            if (obj.EditingElement is TextBox textBox && obj.EditAction == DataGridEditAction.Commit)
            {
                string column = obj.Column.Tag.ToString();
                int rowIndex = obj.Row.GetIndex();
                CustomerItemRow existingCustomer = Customers[rowIndex];
                existingCustomer.Customer.GetType().GetProperty(column).SetValue(existingCustomer.Customer, textBox.Text);
                existingCustomer.ItemDeleteClicked -= this.HandleCustomerDeleted;
                CustomerItemRow vendorItem = new CustomerItemRow
                {
                    Customer = existingCustomer.Customer
                };
                vendorItem.ItemDeleteClicked += this.HandleCustomerDeleted;
                this.Customers.RemoveAt(rowIndex);
                this.Customers.Add(vendorItem);
                this.customerRepository.Create(existingCustomer.Customer);
            }
        }

        private void populateVendors()
        {
            this.clearVendors();
            foreach (var customer in this.customerRepository.GetAllCustomers())
            {
                CustomerItemRow vendorItemRow = new CustomerItemRow
                {
                    Customer = customer
                };
                vendorItemRow.ItemDeleteClicked += HandleCustomerDeleted;
                this.customers.Add(vendorItemRow);
            }
        }

        private void clearVendors()
        {
            foreach (var vendorItem in this.customers)
            {
                vendorItem.ItemDeleteClicked -= this.HandleCustomerDeleted;
            }
            this.Customers.Clear();
        }

        private async Task HandleCustomerDeleted(CustomerItemRow arg)
        {
            if (ContentDialogResult.Primary == await this.messageDialog.ShowAsync())
            {
                arg.ItemDeleteClicked -= HandleCustomerDeleted;
                this.customers.Remove(arg);
                await this.customerRepository.Delete(arg.Customer);
            }

        }

        public ObservableCollection<CustomerItemRow> Customers
        {
            get
            {
                return this.customers;
            }
        }

        public Customer Customer
        {
            get { return this.customer; }
            set
            {
                this.customer = value;
                RaisePropertyChanged("Customer");
            }
        }
    }
}
