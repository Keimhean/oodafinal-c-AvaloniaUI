using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CoffeeShopManagement.Models
{
    public class Order : INotifyPropertyChanged
    {
        private int _orderId;
    // Initialize to now so SQL Server DATETIME columns don't receive DateTime.MinValue
    private DateTime _orderDate = DateTime.Now;
        private string _status = "Pending";
        private decimal _totalAmount;
        private string _customerName = string.Empty;

        public int OrderId
        {
            get => _orderId;
            set { _orderId = value; OnPropertyChanged(); }
        }

        public DateTime OrderDate
        {
            get => _orderDate;
            set { _orderDate = value; OnPropertyChanged(); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set { _totalAmount = value; OnPropertyChanged(); }
        }

        public string CustomerName
        {
            get => _customerName;
            set { _customerName = value; OnPropertyChanged(); }
        }

        public ObservableCollection<OrderItem> Items { get; set; } = new ObservableCollection<OrderItem>();

        public void AddProduct(Product product, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == product.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    Quantity = quantity,
                    UnitPrice = product.Price
                });
            }
            CalculateTotal();
        }

        public void CalculateTotal()
        {
            TotalAmount = Items.Sum(item => item.Subtotal);
            OnPropertyChanged(nameof(TotalAmount));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}