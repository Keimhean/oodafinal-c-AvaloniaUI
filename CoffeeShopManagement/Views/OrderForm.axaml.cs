using Avalonia.Controls;
using Avalonia.Interactivity;
using CoffeeShopManagement.Helpers;
using CoffeeShopManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks; 

namespace CoffeeShopManagement.Views
{
    public partial class OrderForm : Window
    {
        public Order Order { get; private set; }

        public OrderForm(Order order, List<Product> availableProducts)
        {
            Order = order;
            InitializeComponent();
            DataContext = Order;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateOrder())
            {
                Close(Order);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close(null);
        }

        private bool ValidateOrder()
        {
            if (string.IsNullOrWhiteSpace(Order.CustomerName))
            {
                _ = MessageBox.Show(this, "Please enter a customer name", "Validation Error", new[] { "OK" });
                return false;
            }

            if (Order.Items.Count == 0)
            {
                _ = MessageBox.Show(this, "Please add at least one item to the order", "Validation Error", new[] { "OK" });
                return false;
            }

            return true;
        }
    }
}