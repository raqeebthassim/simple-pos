using Microsoft.Toolkit.Uwp.UI.Controls;
using Prism.Commands;
using Prism.Windows.Mvvm;
using SamplePoS.Core.Models;
using SamplePoS.Core.Persistance;
using SamplePoS.ViewModels.ViewComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace SamplePoS.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {
        private ObservableCollection<InventoryItemRow> products;
        private Product product;

        private string selectedCategory;
        private string selectedVendor;

        private readonly IProductRepository productRepository;
        private readonly IVendorRepository vendorRepository;
        private readonly ICategoryRepository categoryRepository;

        public ICommand AddProductCommand { get; private set;}
        public ICommand ResetProductCommand { get; private set;}
        public ICommand SelectionChangedCommand { get; private set;}

        private ContentDialog messageDialog;

        public InventoryViewModel(IProductRepository productRepository, IVendorRepository vendorRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.vendorRepository = vendorRepository;
            this.categoryRepository = categoryRepository;
            this.product = new Product();
            this.products = new ObservableCollection<InventoryItemRow>();
            this.AddProductCommand = new DelegateCommand(HandleAddProduct);
            this.SelectionChangedCommand = new DelegateCommand<SelectionChangedEventArgs>(HandleSelectionChanged);
            this.ResetProductCommand = new DelegateCommand(HandleResetProduct);
            this.messageDialog = new ContentDialog()
            {
                Title = "Confirm",
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "Cancel",
                Content = "Are you sure you wanna delete?"
            };
        }

        private void HandleSelectionChanged(SelectionChangedEventArgs eventArgs)
        {
            if(eventArgs.AddedItems.Count > 0)
            {
                this.Product = ((InventoryItemRow)eventArgs.AddedItems.First()).Product;
                this.SelectedCategory = this.Categories.Where(c => c.Equals(this.Product?.Category?.Name)).FirstOrDefault();
                this.SelectedVendor = this.Vendors.Where(v => v.Equals(this.Product?.Vendor?.Name)).FirstOrDefault();
            }
        }

        private void HandleResetProduct()
        {
            this.Product = new Product();
            this.SelectedCategory = null;
            this.SelectedVendor = null;
        }

        private void HandleAddProduct()
        {
            this.Product.Category = this.categoryRepository.QueryCategories(c => c.Name.Equals(selectedCategory)).FirstOrDefault();
            this.Product.Vendor = this.vendorRepository.GetVendor(v => v.Name.Equals(selectedVendor));
            this.productRepository.Create(Product);
            this.HandleResetProduct();
            RaisePropertyChanged("ProductList");
        }


        public IList<InventoryItemRow> ProductList
        {
            get
            {
                GetInventoryItems();
                return this.products;
            }
        }

        private void GetInventoryItems()
        {
            clearItems();
            foreach (Product p in this.productRepository.GetProductList())
            {
                InventoryItemRow inventoryItemRow = new InventoryItemRow
                {
                    Product = p
                };
                inventoryItemRow.ItemDeleteClicked += this.ProductDeleted;
                this.products.Add(inventoryItemRow);
            }
        }

        private void clearItems()
        {
            foreach (var item in this.products)
            {
                item.ItemDeleteClicked -= this.ProductDeleted;
            }
            this.products.Clear();
        }

        private async Task ProductDeleted(InventoryItemRow arg)
        {
            if(ContentDialogResult.Primary == await this.messageDialog.ShowAsync())
            {
                arg.ItemDeleteClicked -= this.ProductDeleted;
                this.products.Remove(arg);
                await this.productRepository.Delete(arg.Product);
            }
        }

        public IList<string> Categories
        {
            get
            { return this.categoryRepository.GetCategoryNames(); }
        }

        public IList<string> Vendors
        {
            get
            { return this.vendorRepository.GetVendorNames();}
        }

        public string SelectedCategory
        {
            get
            { return this.selectedCategory; }
            set
            {
                this.selectedCategory = value;
                RaisePropertyChanged("SelectedCategory");
            }
        }

        public string SelectedVendor
        {
            get
            { return this.selectedVendor; }
            set
            {
                this.selectedVendor = value;
                RaisePropertyChanged("SelectedVendor");
            }
        }

        public Product Product
        {
            get
            { return this.product; }
            set
            {
                this.product = value;
                RaisePropertyChanged("Product");
            }
        }
    }
}
