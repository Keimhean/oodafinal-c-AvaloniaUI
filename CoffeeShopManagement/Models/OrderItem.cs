using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoffeeShopManagement.Models
{
    public class OrderItem : INotifyPropertyChanged
    {
        private int _orderItemId;
        private int _orderId;
        private int _productId;
        private string _productName = string.Empty;
        private int _quantity;
        private decimal _unitPrice;

        public int OrderItemId
        {
            get => _orderItemId;
            set { _orderItemId = value; OnPropertyChanged(); }
        }

        public int OrderId
        {
            get => _orderId;
            set { _orderId = value; OnPropertyChanged(); }
        }

        public int ProductId
        {
            get => _productId;
            set { _productId = value; OnPropertyChanged(); }
        }

        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(); }
        }

        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); OnPropertyChanged(nameof(Subtotal)); }
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            set { _unitPrice = value; OnPropertyChanged(); OnPropertyChanged(nameof(Subtotal)); }
        }

        public decimal Subtotal => Quantity * UnitPrice;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}