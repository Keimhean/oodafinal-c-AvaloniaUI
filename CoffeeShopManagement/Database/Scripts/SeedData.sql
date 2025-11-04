USE CoffeeShopDB;
GO

-- Seed Categories
IF NOT EXISTS (SELECT 1 FROM Categories)
BEGIN
    INSERT INTO Categories (Name, Description) VALUES 
    ('Coffee', 'Various coffee beverages'),
    ('Tea', 'Different types of tea'),
    ('Bakery', 'Fresh baked goods'),
    ('Specialty', 'Special drinks and items'),
    ('Cold Drinks', 'Iced and cold beverages');
END

-- Seed Products
IF NOT EXISTS (SELECT 1 FROM Products)
BEGIN
    INSERT INTO Products (Name, Description, Price, StockQuantity, Category, IsAvailable) VALUES 
    ('Espresso', 'Strong black coffee shot', 3.50, 100, 'Coffee', 1),
    ('Cappuccino', 'Espresso with steamed milk foam', 4.50, 80, 'Coffee', 1),
    ('Latte', 'Espresso with lots of steamed milk', 5.00, 75, 'Coffee', 1),
    ('Americano', 'Espresso with hot water', 4.00, 90, 'Coffee', 1),
    ('Mocha', 'Chocolate coffee drink', 5.50, 60, 'Coffee', 1),
    ('Green Tea', 'Fresh green tea leaves', 3.00, 50, 'Tea', 1),
    ('Black Tea', 'Strong black tea', 3.00, 45, 'Tea', 1),
    ('Chai Latte', 'Spiced tea with milk', 4.75, 40, 'Tea', 1),
    ('Croissant', 'Buttery French croissant', 2.50, 30, 'Bakery', 1),
    ('Blueberry Muffin', 'Fresh muffin with blueberries', 3.25, 25, 'Bakery', 1),
    ('Chocolate Chip Cookie', 'Classic cookie with chips', 2.00, 40, 'Bakery', 1),
    ('Iced Coffee', 'Chilled coffee beverage', 4.25, 60, 'Cold Drinks', 1),
    ('Iced Tea', 'Refreshing cold tea', 3.50, 55, 'Cold Drinks', 1),
    ('Smoothie', 'Fruit blended drink', 5.75, 35, 'Cold Drinks', 1);
END