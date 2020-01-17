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
    public class VendorsViewModel : ViewModelBase
    {
        private readonly IVendorRepository vendorRepository;
        private ObservableCollection<VendorItemRow> vendors;
        private ContentDialog messageDialog;
        private Vendor vendor;
        public ICommand CellEditEndedCommand { get; private set; }
        public ICommand AddVendorCommand { get; private set; }
        public ICommand ResetVendorCommand { get; private set; }



        public VendorsViewModel(IVendorRepository vendorRepository)
        {
            this.vendorRepository = vendorRepository;
            this.vendors = new ObservableCollection<VendorItemRow>();
            this.messageDialog = new ContentDialog()
            {
                Title = "Confirm",
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "Cancel",
                Content = "Are you sure you wanna delete?"
            };
            this.CellEditEndedCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(HandleCellEditEnded);
            this.AddVendorCommand = new DelegateCommand(HandleAddVendorCommand);
            this.ResetVendorCommand = new DelegateCommand(HandleResetVendorCommand);
            this.Vendor = new Vendor();
            this.populateVendors();
        }

        private void HandleResetVendorCommand()
        {
            this.Vendor = new Vendor();
        }

        private void HandleAddVendorCommand()
        {
            if (this.Vendor != null)
            {
                this.vendorRepository.Create(this.Vendor);
                populateVendors();
                RaisePropertyChanged("Vendors");
                HandleResetVendorCommand();
            }
        }

        private void HandleCellEditEnded(DataGridCellEditEndingEventArgs obj)
        {
            if (obj.EditingElement is TextBox textBox && obj.EditAction == DataGridEditAction.Commit)
            {
                string column = obj.Column.Tag.ToString();
                int rowIndex = obj.Row.GetIndex();
                VendorItemRow existingVendor = Vendors[rowIndex];
                existingVendor.Vendor.GetType().GetProperty(column).SetValue(existingVendor.Vendor, textBox.Text);
                existingVendor.ItemDeleteClicked -= this.HandleVendorDeleted;
                VendorItemRow vendorItem = new VendorItemRow
                {
                    Vendor = existingVendor.Vendor
                };
                vendorItem.ItemDeleteClicked += this.HandleVendorDeleted;
                this.Vendors.RemoveAt(rowIndex);
                this.Vendors.Add(vendorItem);
                this.vendorRepository.Create(existingVendor.Vendor);
            }
        }

        private void populateVendors()
        {
            this.clearVendors();
            foreach (var vendor in this.vendorRepository.GetAllVendors())
            {
                VendorItemRow vendorItemRow = new VendorItemRow
                {
                    Vendor = vendor
                };
                vendorItemRow.ItemDeleteClicked += HandleVendorDeleted;
                this.vendors.Add(vendorItemRow);
            }
        }

        private void clearVendors()
        {
            foreach (var vendorItem in this.vendors)
            {
                vendorItem.ItemDeleteClicked -= this.HandleVendorDeleted;
            }
            this.Vendors.Clear();
        }

        private async Task HandleVendorDeleted(VendorItemRow arg)
        {
            if (ContentDialogResult.Primary == await this.messageDialog.ShowAsync())
            {
                arg.ItemDeleteClicked -= HandleVendorDeleted;
                this.vendors.Remove(arg);
                await this.vendorRepository.Delete(arg.Vendor);
            }

        }

        public ObservableCollection<VendorItemRow> Vendors
        {
            get
            {
                return this.vendors;
            }
        }

        public Vendor Vendor
        {
            get { return this.vendor; }
            set
            {
                this.vendor = value;
                RaisePropertyChanged("Vendor");
            }
        }

    }
}
