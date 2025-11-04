using CoffeeShopManagement.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Services
{
    public class DatabaseService
    {
        private string _connectionString;
        private readonly string _serverConnectionString;

        public DatabaseService()
        {
            // Connection string pointing to server/master for creation operations
            _serverConnectionString = "Server=localhost,1435;Database=master;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true;";
            // Default to server connection until the app DB exists; we'll switch to the app DB after creating it.
            _connectionString = _serverConnectionString;
        }

        private async Task<SqlConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        // Product Methods - FIXED: Use proper column indexing
        public async Task<List<Product>> GetProductsAsync()
        {
            var products = new List<Product>();

            using var connection = await CreateConnectionAsync();
            const string sql = @"
                SELECT ProductId, Name, Description, Price, StockQuantity, Category, IsAvailable 
                FROM Products 
                ORDER BY Name";

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(new Product
                {
                    ProductId = reader.GetInt32(0),  // ProductId
                    Name = reader.GetString(1),      // Name
                    Description = reader.GetString(2), // Description
                    Price = reader.GetDecimal(3),    // Price
                    StockQuantity = reader.GetInt32(4), // StockQuantity
                    Category = reader.GetString(5),  // Category
                    IsAvailable = reader.GetBoolean(6) // IsAvailable
                });
            }

            return products;
        }

        public async Task<int> AddProductAsync(Product product)
        {
            using var connection = await CreateConnectionAsync();
            const string sql = @"
                INSERT INTO Products (Name, Description, Price, StockQuantity, Category, IsAvailable) 
                VALUES (@Name, @Description, @Price, @StockQuantity, @Category, @IsAvailable);
                SELECT SCOPE_IDENTITY();";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Description", product.Description ?? "");
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
            command.Parameters.AddWithValue("@Category", product.Category);
            command.Parameters.AddWithValue("@IsAvailable", product.IsAvailable);

            var result = await command.ExecuteScalarAsync();
            return result == null ? 0 : Convert.ToInt32(result);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            using var connection = await CreateConnectionAsync();
            const string sql = @"
                UPDATE Products 
                SET Name = @Name, Description = @Description, Price = @Price, 
                    StockQuantity = @StockQuantity, Category = @Category, IsAvailable = @IsAvailable 
                WHERE ProductId = @ProductId";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProductId", product.ProductId);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Description", product.Description ?? "");
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
            command.Parameters.AddWithValue("@Category", product.Category);
            command.Parameters.AddWithValue("@IsAvailable", product.IsAvailable);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            using var connection = await CreateConnectionAsync();
            const string sql = "DELETE FROM Products WHERE ProductId = @ProductId";
            
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProductId", productId);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            var products = new List<Product>();

            using var connection = await CreateConnectionAsync();
            const string sql = @"
                SELECT ProductId, Name, Description, Price, StockQuantity, Category, IsAvailable 
                FROM Products 
                WHERE Name LIKE @SearchTerm OR Description LIKE @SearchTerm OR Category LIKE @SearchTerm
                ORDER BY Name";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                products.Add(new Product
                {
                    ProductId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    StockQuantity = reader.GetInt32(4),
                    Category = reader.GetString(5),
                    IsAvailable = reader.GetBoolean(6)
                });
            }

            return products;
        }

        // Order Methods
        public async Task<List<Order>> GetOrdersAsync()
        {
            var orders = new List<Order>();

            using var connection = await CreateConnectionAsync();
            const string sql = @"
                SELECT OrderId, OrderDate, Status, TotalAmount, CustomerName 
                FROM Orders 
                ORDER BY OrderDate DESC";

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var order = new Order
                {
                    OrderId = reader.GetInt32(0),
                    OrderDate = reader.GetDateTime(1),
                    Status = reader.GetString(2),
                    TotalAmount = reader.GetDecimal(3),
                    CustomerName = reader.GetString(4)
                };

                // Load order items
                order.Items = await GetOrderItemsAsync(order.OrderId);
                orders.Add(order);
            }

            return orders;
        }

        private async Task<ObservableCollection<OrderItem>> GetOrderItemsAsync(int orderId)
        {
            var items = new ObservableCollection<OrderItem>();

            using var connection = await CreateConnectionAsync();
            const string sql = @"
                SELECT OrderItemId, ProductId, ProductName, Quantity, UnitPrice 
                FROM OrderItems 
                WHERE OrderId = @OrderId";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@OrderId", orderId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                items.Add(new OrderItem
                {
                    OrderItemId = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    ProductName = reader.GetString(2),
                    Quantity = reader.GetInt32(3),
                    UnitPrice = reader.GetDecimal(4)
                });
            }

            return items;
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            using var connection = await CreateConnectionAsync();
            
            // FIXED: Use synchronous BeginTransaction for Microsoft.Data.SqlClient
            using var transaction = connection.BeginTransaction();

            try
            {
                // Insert order
                const string orderSql = @"
                    INSERT INTO Orders (OrderDate, Status, TotalAmount, CustomerName) 
                    VALUES (@OrderDate, @Status, @TotalAmount, @CustomerName);
                    SELECT SCOPE_IDENTITY();";

                using var orderCommand = new SqlCommand(orderSql, connection, transaction);
                orderCommand.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                orderCommand.Parameters.AddWithValue("@Status", order.Status);
                orderCommand.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                orderCommand.Parameters.AddWithValue("@CustomerName", order.CustomerName);

                var result = await orderCommand.ExecuteScalarAsync();
                var orderId = result == null ? 0 : Convert.ToInt32(result);

                // Insert order items
                const string itemSql = @"
                    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice) 
                    VALUES (@OrderId, @ProductId, @ProductName, @Quantity, @UnitPrice)";

                foreach (var item in order.Items)
                {
                    using var itemCommand = new SqlCommand(itemSql, connection, transaction);
                    itemCommand.Parameters.AddWithValue("@OrderId", orderId);
                    itemCommand.Parameters.AddWithValue("@ProductId", item.ProductId);
                    itemCommand.Parameters.AddWithValue("@ProductName", item.ProductName);
                    itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                    itemCommand.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);

                    await itemCommand.ExecuteNonQueryAsync();
                }

                transaction.Commit();
                return orderId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            using var connection = await CreateConnectionAsync();
            const string sql = "UPDATE Orders SET Status = @Status WHERE OrderId = @OrderId";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@OrderId", orderId);
            command.Parameters.AddWithValue("@Status", status);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        // Category Methods
        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = new List<Category>();

            using var connection = await CreateConnectionAsync();
            const string sql = "SELECT CategoryId, Name, Description FROM Categories ORDER BY Name";

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    CategoryId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2)
                });
            }

            return categories;
        }

        // Initialize Database
        public async Task InitializeDatabaseAsync()
        {
            // 1) Connect to master and ensure CoffeeShopDB exists
            await using (var serverConn = new SqlConnection(_serverConnectionString))
            {
                await serverConn.OpenAsync();
                var ensureDbSql = "IF DB_ID('CoffeeShopDB') IS NULL CREATE DATABASE CoffeeShopDB;";
                using var ensureCmd = new SqlCommand(ensureDbSql, serverConn);
                await ensureCmd.ExecuteNonQueryAsync();
            }

            // 2) Switch the service connection string to the application database
            _connectionString = "Server=localhost,1435;Database=CoffeeShopDB;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true;";

            // 3) Connect to CoffeeShopDB and create tables + seed data
            await using (var appConn = new SqlConnection(_connectionString))
            {
                await appConn.OpenAsync();

                var createTablesSql = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
                    CREATE TABLE Products (
                        ProductId INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(100) NOT NULL,
                        Description NVARCHAR(500),
                        Price DECIMAL(18,2) NOT NULL,
                        StockQuantity INT NOT NULL DEFAULT 0,
                        Category NVARCHAR(50) NOT NULL,
                        IsAvailable BIT NOT NULL DEFAULT 1,
                        CreatedDate DATETIME2 DEFAULT GETDATE()
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
                    CREATE TABLE Categories (
                        CategoryId INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(50) NOT NULL,
                        Description NVARCHAR(200)
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Orders' AND xtype='U')
                    CREATE TABLE Orders (
                        OrderId INT IDENTITY(1,1) PRIMARY KEY,
                        OrderDate DATETIME2 NOT NULL,
                        Status NVARCHAR(20) NOT NULL,
                        TotalAmount DECIMAL(18,2) NOT NULL,
                        CustomerName NVARCHAR(100) NOT NULL
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='OrderItems' AND xtype='U')
                    CREATE TABLE OrderItems (
                        OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
                        OrderId INT NOT NULL,
                        ProductId INT NOT NULL,
                        ProductName NVARCHAR(100) NOT NULL,
                        Quantity INT NOT NULL,
                        UnitPrice DECIMAL(18,2) NOT NULL,
                        FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE
                    );";

                using var command = new SqlCommand(createTablesSql, appConn);
                await command.ExecuteNonQueryAsync();

                // Seed initial data using the open application connection
                await SeedInitialDataAsync(appConn);
            }
        }

        private async Task SeedInitialDataAsync(SqlConnection connection)
        {
            // Check if products already exist
            var checkSql = "SELECT COUNT(*) FROM Products";
            using var checkCommand = new SqlCommand(checkSql, connection);
            var result = await checkCommand.ExecuteScalarAsync();
            var count = result == null ? 0 : (int)result; // កែហើយ៖ ការបម្លែងមានសុវត្ថិភាពពី null
            if (count == 0)
            {
                // Seed Categories
                var categoriesSql = @"
                    INSERT INTO Categories (Name, Description) VALUES 
                    ('Coffee', 'Various coffee beverages'),
                    ('Tea', 'Different types of tea'),
                    ('Bakery', 'Fresh baked goods'),
                    ('Specialty', 'Special drinks and items')";

                using var catCommand = new SqlCommand(categoriesSql, connection);
                await catCommand.ExecuteNonQueryAsync();

                // Seed Products
                var productsSql = @"
                    INSERT INTO Products (Name, Description, Price, StockQuantity, Category, IsAvailable) VALUES 
                    ('Espresso', 'Strong black coffee', 3.50, 100, 'Coffee', 1),
                    ('Cappuccino', 'Espresso with steamed milk', 4.50, 80, 'Coffee', 1),
                    ('Latte', 'Espresso with lots of milk', 5.00, 75, 'Coffee', 1),
                    ('Green Tea', 'Fresh green tea', 3.00, 50, 'Tea', 1),
                    ('Croissant', 'Buttery French croissant', 2.50, 30, 'Bakery', 1),
                    ('Blueberry Muffin', 'Fresh muffin with blueberries', 3.25, 25, 'Bakery', 1),
                    ('Iced Coffee', 'Chilled coffee beverage', 4.25, 60, 'Coffee', 1),
                    ('Chai Latte', 'Spiced tea with milk', 4.75, 45, 'Tea', 1)";

                using var prodCommand = new SqlCommand(productsSql, connection);
                await prodCommand.ExecuteNonQueryAsync();
            }
        }
    }
}