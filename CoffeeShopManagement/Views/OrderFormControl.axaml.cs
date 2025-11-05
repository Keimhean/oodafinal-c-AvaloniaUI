using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using CoffeeShopManagement.Helpers;
using CoffeeShopManagement.Models;
using System;

namespace CoffeeShopManagement.Views
{
    public partial class OrderFormControl : UserControl
    {
        public Order Order { get; private set; } = new Order();

        public event EventHandler<Order?>? Saved;
        public event EventHandler? Canceled;

        public OrderFormControl()
        {
            InitializeComponent();
        }

        public void SetOrder(Order order)
        {
            Order = order;
            DataContext = Order;
        }

        private void Save_Click(object? sender, RoutedEventArgs e)
        {
            if (ValidateOrder())
            {
                Saved?.Invoke(this, Order);
            }
        }

        private void Cancel_Click(object? sender, RoutedEventArgs e)
        {
            Canceled?.Invoke(this, EventArgs.Empty);
        }

        private bool ValidateOrder()
        {
            if (string.IsNullOrWhiteSpace(Order.CustomerName))
            {
                var owner = this.FindAncestorOfType<Window>() ?? (this.VisualRoot as Window);
                _ = MessageBox.Show(owner, "Please enter a customer name", "Validation Error", new[] { "OK" });
                return false;
            }

            if (Order.Items.Count == 0)
            {
                var owner = this.FindAncestorOfType<Window>() ?? (this.VisualRoot as Window);
                _ = MessageBox.Show(owner, "Please add at least one item to the order", "Validation Error", new[] { "OK" });
                return false;
            }

            return true;
        }
    }
}
