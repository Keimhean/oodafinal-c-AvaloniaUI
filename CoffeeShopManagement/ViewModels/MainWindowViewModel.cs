using CoffeeShopManagement.Models;
using CoffeeShopManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CoffeeShopManagement.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private const string ActiveButtonColor = "#615FFF";
        private const string DefaultButtonColor = "#20FFFFFF";
        
        [ObservableProperty]
        private ObservableCollection<Product> _products = new();

        [ObservableProperty]
        private ObservableCollection<Order> _orders = new();

        [ObservableProperty]
        private ObservableCollection<Category> _categories = new();

        [ObservableProperty]
        private Product? _selectedProduct;

        [ObservableProperty]
        private string _statusMessage = "Ready";

        [ObservableProperty]
        private bool _isLoading;

        // UI fields used for bindings/commands
        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private Order? _selectedOrder;

        [ObservableProperty]
        private string _addProductButtonColor = DefaultButtonColor;

        [ObservableProperty]
        private string _updateProductButtonColor = DefaultButtonColor;

        [ObservableProperty]
        private string _deleteProductButtonColor = DefaultButtonColor;

        [ObservableProperty]
        private string _refreshButtonColor = DefaultButtonColor;

            public MainWindowViewModel(DatabaseService databaseService)
            {
                _databaseService = databaseService;
                _ = InitializeAsync();
            }

        private async Task InitializeAsync()
        {
            await InitializeDatabase();
            await LoadData();
        }

        private async Task InitializeDatabase()
        {
            try
            {
                await _databaseService.InitializeDatabaseAsync();
                StatusMessage = "Database initialized successfully";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Database initialization failed: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task LoadData()
        {
            RefreshButtonColor = ActiveButtonColor;
            IsLoading = true;
            try
            {
                var products = await _databaseService.GetProductsAsync();
                var orders = await _databaseService.GetOrdersAsync();
                var categories = await _databaseService.GetCategoriesAsync();

                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }

                Orders.Clear();
                foreach (var order in orders)
                {
                    Orders.Add(order);
                }

                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                StatusMessage = $"Loaded {Products.Count} products and {Orders.Count} orders";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                RefreshButtonColor = DefaultButtonColor;
            }
        }

        [RelayCommand]
        private async Task AddProduct()
        {
            AddProductButtonColor = ActiveButtonColor;
            try
            {
                var newProduct = new Product { Name = "New Product", Price = 0, StockQuantity = 0 };
                var productId = await _databaseService.AddProductAsync(newProduct);
                newProduct.ProductId = productId;
                Products.Add(newProduct);
                SelectedProduct = newProduct;
                StatusMessage = "Product added successfully";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error adding product: {ex.Message}";
            }
            finally
            {
                AddProductButtonColor = DefaultButtonColor;
            }
        }

        [RelayCommand]
        private async Task UpdateProduct()
        {
            if (SelectedProduct == null)
            {
                StatusMessage = "Please select a product to update";
                return;
            }
            UpdateProductButtonColor = ActiveButtonColor;
            try
            {
                await _databaseService.UpdateProductAsync(SelectedProduct);
                StatusMessage = "Product updated successfully";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating product: {ex.Message}";
            }
            finally
            {
                UpdateProductButtonColor = DefaultButtonColor;
            }
        }

        [RelayCommand]
        private async Task DeleteProduct()
        {
            if (SelectedProduct == null)
            {
                StatusMessage = "Please select a product to delete";
                return;
            }
            DeleteProductButtonColor = ActiveButtonColor;
            try
            {
                await _databaseService.DeleteProductAsync(SelectedProduct.ProductId);
                Products.Remove(SelectedProduct);
                SelectedProduct = null;
                StatusMessage = "Product deleted successfully";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error deleting product: {ex.Message}";
            }
            finally
            {
                DeleteProductButtonColor = DefaultButtonColor;
            }
        }

        [RelayCommand]
        private async Task SearchProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadData();
                return;
            }

            IsLoading = true;
            try
            {
                var filteredProducts = await _databaseService.SearchProductsAsync(SearchText);
                Products.Clear();
                foreach (var product in filteredProducts)
                {
                    Products.Add(product);
                }
                StatusMessage = $"Found {filteredProducts.Count} products matching '{SearchText}'";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error searching products: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task CreateOrder()
        {
            var newOrder = new Order
            {
                // Ensure OrderDate is within SQL Server range
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = 0m,
                CustomerName = string.Empty
            };

            try
            {
                var orderId = await _databaseService.CreateOrderAsync(newOrder);
                newOrder.OrderId = orderId;
                Orders.Insert(0, newOrder);
                SelectedOrder = newOrder;
                StatusMessage = "Order created successfully";
            }
            catch (Exception ex)
            {
                // If Orders table was missing (common on first run/race), try to initialize DB and retry once.
                if (ex.Message.Contains("Invalid object name 'Orders'", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        await _databaseService.InitializeDatabaseAsync();
                        var orderId = await _databaseService.CreateOrderAsync(newOrder);
                        newOrder.OrderId = orderId;
                        Orders.Insert(0, newOrder);
                        SelectedOrder = newOrder;
                        StatusMessage = "Order created successfully (after DB initialization)";
                        return;
                    }
                    catch (Exception retryEx)
                    {
                        StatusMessage = $"Error creating order after DB init: {retryEx.Message}";
                        return;
                    }
                }

                StatusMessage = $"Error creating order: {ex.Message}";
            }
        }

        // SearchText change handling removed to avoid partial method generator mismatch; keep explicit Search button/command instead.
    }
}