using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using CoffeeShopManagement.Constants;
using CoffeeShopManagement.Helpers;
using CoffeeShopManagement.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Views
{
    public partial class MainWindow : Window
    {
        private readonly DatabaseService _databaseService;

        public MainWindow(DatabaseService databaseService, ViewModels.MainWindowViewModel viewModel)
        {
            _databaseService = databaseService;
            InitializeComponent();
            ApplyLiquidGlassEffects();

            // Set the DataContext to the DI-resolved view model so the XAML does not need to create it.
            this.DataContext = viewModel;
        }

        private void ApplyLiquidGlassEffects()
        {
            this.TransparencyLevelHint = new List<WindowTransparencyLevel> { WindowTransparencyLevel.AcrylicBlur };
            this.Background = new SolidColorBrush(Color.FromArgb(30, 255, 255, 255));
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _databaseService.InitializeDatabaseAsync();
            }
            catch (Exception ex)
            {
                await MessageBox.Show(this, $"Database initialization failed: {ex.Message}", "Error", new[] { "OK" });
            }
        }

        private void HideOrderOverlay()
        {
            var overlay = this.FindControl<Grid>("OrderOverlay");
            var host = this.FindControl<ContentControl>("OrderHost");
            if (host != null)
                host.Content = null;
            if (overlay != null)
                overlay.IsVisible = false;
        }

        private void ShowOrderOverlay(Control content)
        {
            var overlay = this.FindControl<Grid>("OrderOverlay");
            var host = this.FindControl<ContentControl>("OrderHost");
            if (host != null)
                host.Content = content;
            if (overlay != null)
                overlay.IsVisible = true;
        }

        private void NewOrder_ShowEmbedded_Click(object? sender, RoutedEventArgs e)
        {
            if (this.DataContext is ViewModels.MainWindowViewModel vm)
            {
                var newOrder = new Models.Order
                {
                    OrderDate = DateTime.Now,
                    Status = OrderStatus.Pending,
                    TotalAmount = 0m,
                    CustomerName = string.Empty
                };

                var control = new OrderFormControl();
                control.SetOrder(newOrder);

                control.Saved += async (_, order) =>
                {
                    try
                    {
                        var orderId = await _databaseService.CreateOrderAsync(order!);
                        order!.OrderId = orderId;
                        vm.Orders.Insert(0, order);
                        vm.SelectedOrder = order;
                        vm.StatusMessage = "Order created successfully";
                    }
                    catch (Exception ex)
                    {
                        vm.StatusMessage = $"Error creating order: {ex.Message}";
                        _ = MessageBox.Show(this, vm.StatusMessage, "Error", new[] { "OK" });
                    }
                    finally
                    {
                        HideOrderOverlay();
                    }
                };

                control.Canceled += (_, __) => HideOrderOverlay();

                ShowOrderOverlay(control);
            }
        }
    }
}