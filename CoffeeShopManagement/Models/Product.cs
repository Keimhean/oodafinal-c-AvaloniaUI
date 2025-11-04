using CoffeeShopManagement.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CoffeeShopManagement.Models
{
    public class Product : INotifyPropertyChanged
    {
        private int _productId;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private decimal _price;
        private int _stockQuantity;
        private string _category = ProductCategory.Coffee;
        private bool _isAvailable = true;

        public int ProductId
        {
            get => _productId;
            set { _productId = value; OnPropertyChanged(); }
        }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 100 characters")]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        [Range(0.01, 1000000, ErrorMessage = "Price must be between 0.01 and 1,000,000")]
        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        [Range(0, 100000, ErrorMessage = "Stock quantity must be between 0 and 100,000")]
        public int StockQuantity
        {
            get => _stockQuantity;
            set { _stockQuantity = value; OnPropertyChanged(); OnPropertyChanged(nameof(StockStatus)); }
        }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            set { _isAvailable = value; OnPropertyChanged(); }
        }

        public string StockStatus => StockQuantity > 10 ? StockStatusText.InStock 
            : StockQuantity > 0 ? StockStatusText.LowStock 
            : StockStatusText.OutOfStock;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual decimal CalculateDiscount(decimal discountPercentage)
        {
            return Price * (1 - discountPercentage / 100);
        }

        public override string ToString()
        {
            return $"{Name} - ${Price} ({StockStatus})";
        }
    }
}