using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SamplePoS.Core.Models;
using SamplePoS.Core.Persistance;
using SamplePoS.Types;
using SamplePoS.ViewModels.ViewComponents;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SamplePoS.ViewModels
{
    public class BillingViewModel : ViewModelBase
    {
        private readonly IProductRepository productRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IOrderRepository orderRepository;
        private IList<string> suggestedProducts;
        private IList<string> suggestedCustomers;
        private Customer customer;
        private ObservableCollection<OrderItemListView> orderItems;
        private string searchText;
        private string discountText;
        private decimal billTotal;
        private decimal billSubTotal;
        private DateTime billingDate;
        private string customerSearchText;

        private ContentDialog quantityDialog;
        private ContentDialog messageDialog;
        private TextBox decimalTextBox;
        private TextBox defaultTextBox;
        public ICommand TextChangedCommand { get; private set; }
        public ICommand CustomerTextChangedCommand { get; private set; }
        public ICommand QuerySubmittedCommand { get; private set; }
        public ICommand CustomerQuerySubmittedCommand { get; private set; }
        public ICommand CellEditEndedCommand { get; private set; }
        public ICommand AddCustomerCommand { get; private set; }
        public ICommand SelectedDatesChangedCommand { get; private set; }
        public ICommand ProceedBillingCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }

        public BillingViewModel(
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository)
        {
            this.productRepository = productRepository;
            this.customerRepository = customerRepository;
            this.orderRepository = orderRepository;
            this.TextChangedCommand = new DelegateCommand<AutoSuggestBoxTextChangedEventArgs>(HandleTextChangedEvent);
            this.CustomerTextChangedCommand = new DelegateCommand<AutoSuggestBoxTextChangedEventArgs>(HandleCustomerTextChangedEvent);
            this.QuerySubmittedCommand = new DelegateCommand<AutoSuggestBoxQuerySubmittedEventArgs>(HandleQuerySubmittedEvent);
            this.CustomerQuerySubmittedCommand = new DelegateCommand<AutoSuggestBoxQuerySubmittedEventArgs>(HandleCustomerQuerySubmittedEvent);
            this.CellEditEndedCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(HandleCellEditEnded);
            this.SelectedDatesChangedCommand = new DelegateCommand<CalendarViewSelectedDatesChangedEventArgs>(HandleSelectedDatesChanged);
            this.ProceedBillingCommand = new DelegateCommand(HandleProceedEvent);
            this.AddCustomerCommand = new DelegateCommand(HandleAddCustomerEvent);
            this.ResetCommand = new DelegateCommand(ResetOrderView);
            this.orderItems = new ObservableCollection<OrderItemListView>();
            this.orderItems.CollectionChanged += OrderItems_CollectionChanged;
            this.decimalTextBox = new TextBox {
                Height = (double)App.Current.Resources["TextControlThemeMinHeight"],
                Text = "1"};
            TextBoxRegex.SetValidationMode(decimalTextBox, TextBoxRegex.ValidationMode.Dynamic);
            TextBoxRegex.SetValidationType(decimalTextBox, TextBoxRegex.ValidationType.Decimal);

            this.defaultTextBox = new TextBox();
            TextBoxRegex.SetValidationMode(defaultTextBox, TextBoxRegex.ValidationMode.Dynamic);
            TextBoxRegex.SetValidationType(defaultTextBox, TextBoxRegex.ValidationType.Characters);
        }

        private void HandleSelectedDatesChanged(CalendarViewSelectedDatesChangedEventArgs obj)
        {
            this.BillingDate = obj.AddedDates[0].LocalDateTime;
        }

        private void HandleCustomerQuerySubmittedEvent(AutoSuggestBoxQuerySubmittedEventArgs obj)
        {
            this.Customer = obj != null ? this.customerRepository.GetCustomer(c => c.Name == obj.QueryText): this.customerRepository.GetCustomer(c => c.Name == this.CustomerSearchText);
            this.CustomerSearchText = string.Empty;
        }

        private void HandleCustomerTextChangedEvent(AutoSuggestBoxTextChangedEventArgs e)
        {
            if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                this.SuggestedCustomers = this.customerRepository
                    .GetCustomerNames()
                    .Where(p => p.ToLower().StartsWith(this.CustomerSearchText.ToLower()))
                    .ToList();
            }
        }

        private async void HandleAddCustomerEvent()
        {
            this.quantityDialog.Content = this.defaultTextBox;
            this.quantityDialog.Title = "Add Customer";
            if(ContentDialogResult.Primary == await this.ShowQuantityDialog())
            {
                TextBox textBox = (TextBox)this.quantityDialog.Content;
                if(!textBox.Text.Equals(string.Empty))
                {
                    await this.customerRepository.Create(new Customer { Name = textBox.Text});
                    this.customerSearchText = textBox.Text;
                    this.HandleCustomerQuerySubmittedEvent(null);
                }
            }
        }

        private void OrderItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
              this.CalculateTotal();
        }

        private void CalculateTotal()
        {
            if (this.OrderItems.Count > 0)
            {
                billTotal = this.OrderItems.Sum(o => o.OrderItem.SubTotal);
                if (decimal.TryParse(this.discountText, out decimal discount))
                {
                    billSubTotal = billTotal;
                    billTotal = this.GetSubTotal(billTotal, 1, discount);
                }
            }
            RaisePropertyChanged("BillTotal");
        }

        public async void HandleProceedEvent()
        {
            this.messageDialog.Content = "Do you want to proceed.";
            decimal.TryParse(this.DiscountText, out decimal orderDiscount);
            if (ContentDialogResult.Primary == await this.messageDialog.ShowAsync())
            {
                Order order = new Order()
                {
                    Customer = this.customer,
                    OrderDiscount = orderDiscount,
                    OrderItems = this.OrderItems.Select(ot => ot.OrderItem).ToList(),
                    SubTotal = this.billSubTotal,
                    Time = this.BillingDate == null? DateTime.Now: this.BillingDate,
                    Total = billTotal
                };
                foreach (var orderItem in this.OrderItems.Select(p => p.OrderItem))
                {
                    orderItem.Product.Quantity -= orderItem.Quantity;
                }
                await this.orderRepository.Create(order);
                this.ResetOrderView();
            }
        }

        private void HandleCellEditEnded(DataGridCellEditEndingEventArgs obj)
        {
            if(obj.EditingElement is TextBox textBox)
            {
                string value = textBox.Text;
                if (decimal.TryParse(value, out decimal decimalValue))
                {
                    Enum.TryParse(obj.Column.Tag.ToString(), out OrderColumnType orderColumnType);
                    int rowIndex = obj.Row.GetIndex();
                    OrderItemListView existingOrderItem = OrderItems[rowIndex];
                    existingOrderItem.OrderItem.GetType().GetProperty(orderColumnType.ToString()).SetValue(existingOrderItem.OrderItem, decimalValue);
                    var column = orderColumnType.ToString();
                    OrderItemListView updatedOrderItem = this.GetDuplicatedItem(existingOrderItem);
                    this.DeletOrderItem(existingOrderItem);
                    this.OrderItems.Insert(rowIndex, updatedOrderItem);
                    existingOrderItem.Dispose();
                }
            }
        }

        private OrderItemListView GetDuplicatedItem(OrderItemListView oldOrderItem)
        {
            OrderItemListView orderItemListView = new OrderItemListView
            {
                OrderItem = new OrderItem
                {
                    Product = oldOrderItem.OrderItem.Product,
                    ProductId = oldOrderItem.OrderItem.ProductId,
                    Quantity = oldOrderItem.OrderItem.Quantity,
                    LineDiscount = oldOrderItem.OrderItem.LineDiscount,
                    SubTotal = oldOrderItem.OrderItem.SubTotal
                }
            };
            orderItemListView.OrderItem.SubTotal = this.GetSubTotal(orderItemListView.OrderItem.Product.SellingPrice, orderItemListView.OrderItem.Quantity, orderItemListView.OrderItem.LineDiscount);
            orderItemListView.ItemDeleteClicked += this.HandleItemDeleted;
            oldOrderItem.ItemDeleteClicked -= this.HandleItemDeleted;
            return orderItemListView;
        }

        private decimal GetSubTotal(decimal price, decimal quantity, decimal discount)
        {
            discount = discount > 0 ? (discount / 100) * price * quantity: 0;
            return discount > 0 ?(price * quantity) - discount : quantity * price;
        }

        private void InitializeView()
        {
            this.SuggestedProducts = this.productRepository.GetProductNames();
            this.SuggestedCustomers = this.customerRepository.GetCustomerNames();
            this.InitMessageDialog();
        }

        public void HandleQuerySubmittedEvent(AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            if (e.ChosenSuggestion != null)
            {
                this.AddProduct(this.GetProductByName(e.ChosenSuggestion.ToString()));
            }

        }

        private Product GetProductByName(string name)
        {
            return this.productRepository.GetProductList().FirstOrDefault(p => p.Name.Equals(name));
        }

        private async void AddProduct(Product product)
        {
            this.decimalTextBox.Text = "1";
            this.quantityDialog.Content = this.decimalTextBox;
            this.quantityDialog.Title = "Enter quantity";
            ContentDialogResult result = await this.ShowQuantityDialog();
            if (result == ContentDialogResult.Primary)
            {
                TextBox input = (TextBox)quantityDialog.Content;
                decimal.TryParse(input.Text, out decimal quantity);
                if(quantity == 0)
                {
                    quantity = 1;
                }
                if (quantity <= product.Quantity)
                {
                    if (OrderItems.Any(p => p.OrderItem.ProductId == product.ProductId))
                    {
                        OrderItemListView existingItem = OrderItems.FirstOrDefault(p => p.OrderItem.ProductId == product.ProductId);
                        int index = OrderItems.IndexOf(existingItem);
                        existingItem.OrderItem.Quantity += quantity;
                        OrderItemListView orderItemListView = this.GetDuplicatedItem(existingItem);
                        OrderItems.RemoveAt(index);
                        OrderItems.Insert(index, orderItemListView);
                    }
                    else
                    {
                        OrderItemListView orderItem = new OrderItemListView
                        {
                            OrderItem = new OrderItem
                            {
                                Product = product,
                                ProductId = product.ProductId,
                                Quantity = quantity,
                                LineDiscount = 0,
                                SubTotal = this.GetSubTotal(product.SellingPrice, quantity, 0)
                            }
                        };
                        orderItem.ItemDeleteClicked += this.HandleItemDeleted;
                        OrderItems.Add(orderItem);
                    }
                    RaisePropertyChanged("OrderItems");
                    RaisePropertyChanged("IsProceedEnabled");

                }
                else
                {
                    this.messageDialog.Content = "Insufficient quantity !";
                    await this.messageDialog.ShowAsync();
                }
                this.SearchText = product.Name;
            }
            this.SearchText = string.Empty;
        }

        public void DeletOrderItem(OrderItemListView orderItem)
        {
            int index = this.OrderItems.IndexOf(orderItem);
            this.OrderItems[index].ItemDeleteClicked -= this.HandleItemDeleted;
            this.OrderItems.RemoveAt(index);
            orderItem.Dispose();
            RaisePropertyChanged("IsProceedEnabled");
        }

        public async Task HandleItemDeleted(OrderItemListView obj)
        {
            ContentDialogResult result = await this.messageDialog.ShowAsync();
            if(result == ContentDialogResult.Primary)
            {
                this.DeletOrderItem(obj);
            }
        }

        public void HandleTextChangedEvent(AutoSuggestBoxTextChangedEventArgs e)
        {
            if(e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                this.SuggestedProducts = this.productRepository
                    .GetProductList()
                    .Where(p => p.Name.ToLower().StartsWith(this.SearchText.ToLower()))
                    .Select(p => p.Name)
                    .ToList();
            }
        }

        private void InitMessageDialog()
        {
            this.quantityDialog = new ContentDialog()
            {
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = "Add",
                SecondaryButtonText = "Cancel",
                Content = this.decimalTextBox
            }; 
            this.messageDialog = new ContentDialog()
            {
                Title = "Confirm",
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "Cancel",
                Content = "Are you sure you wanna delete?"
            };
        }


        public async Task<ContentDialogResult> ShowQuantityDialog()
        {
            return await this.quantityDialog.ShowAsync();
        }

        public IList<string> SuggestedProducts
        {
            get
            { return this.suggestedProducts; }
            set
            {
                this.suggestedProducts = value;
                RaisePropertyChanged("SuggestedProducts");
            }
        }

        public IList<string> SuggestedCustomers
        {
            get
            { return this.suggestedCustomers; }
            set
            {
                this.suggestedCustomers = value;
                RaisePropertyChanged("SuggestedCustomers");
            }
        }

        public ObservableCollection<OrderItemListView> OrderItems
        {
            get
            { return this.orderItems; }
            set
            {
                this.orderItems = value;
                RaisePropertyChanged("OrderItems");
            }
        }

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

        public string CustomerSearchText
        {
            get
            { return this.customerSearchText; }
            set
            {
                this.customerSearchText = value;
                RaisePropertyChanged("CustomerSearchText");
            }
        }

        public string DiscountText
        {
            get
            { return this.discountText; }
            set
            {
                this.discountText = value;
                this.CalculateTotal();
                RaisePropertyChanged("DiscountText");
            }
        }

        public Customer Customer
        {
            get
            { return this.customer; }
            set
            {
                this.customer = value;
                RaisePropertyChanged("Customer");
            }
        }

        public string BillTotal
        {
            get
            {
                return this.billTotal.ToString();
            }
        }

        public DateTime BillingDate
        {
            get
            { return this.billingDate; }
            set
            {
                this.billingDate = value;
                RaisePropertyChanged("BillingDate");
            }
        }

        public bool IsProceedEnabled
        {
            get
            {
                return this.OrderItems.Count > 0;
            }
        }

        private void ResetOrderView()
        {
            foreach (var orderItem in OrderItems)
            {
                orderItem.ItemDeleteClicked -= HandleItemDeleted;
                //orderItem.Dispose();
            };
            OrderItems.Clear();
            this.SearchText = string.Empty;
            this.DiscountText = string.Empty;
            this.billTotal = 0;
            this.billSubTotal = 0;
            this.BillingDate = DateTime.Now;
            this.CustomerSearchText = string.Empty;
            this.Customer = null;
            RaisePropertyChanged("BillTotal");
            RaisePropertyChanged("IsProceedEnabled");
        }


        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                InitializeView();
            });
        }
    }
}
