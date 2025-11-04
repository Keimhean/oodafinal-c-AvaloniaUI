using Avalonia.Controls;
using Avalonia.Interactivity;
using CoffeeShopManagement.Helpers;
using CoffeeShopManagement.Models;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Views
{
    public partial class ProductForm : Window
    {
        public Product Product { get; private set; }

        public ProductForm(Product? product = null)
        {
            Product = product ?? new Product();
            InitializeComponent();
            DataContext = Product;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateProduct())
            {
                Close(Product);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close(null);
        }

        private bool ValidateProduct()
        {
            if (string.IsNullOrWhiteSpace(Product.Name))
            {
                _ = MessageBox.Show(this, "Please enter a product name", "Validation Error", new[] { "OK" });
                return false;
            }

            if (Product.Price <= 0)
            {
                _ = MessageBox.Show(this, "Please enter a valid price", "Validation Error", new[] { "OK" });
                return false;
            }

            if (Product.StockQuantity < 0)
            {
                _ = MessageBox.Show(this, "Stock quantity cannot be negative", "Validation Error", new[] { "OK" });
                return false;
            }

            return true;
        }
    }
}