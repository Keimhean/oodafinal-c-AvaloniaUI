using Avalonia.Controls;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Helpers
{
    public static class MessageBox
    {
        public static async Task<string> Show(Window owner, string message, string title, string[] buttons)
        {
            var dialog = new Window
            {
                Title = title,
                Width = 400,
                Height = 150,
                CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var panel = new StackPanel { Margin = new Avalonia.Thickness(20) };
            panel.Children.Add(new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap });

            var buttonPanel = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Margin = new Avalonia.Thickness(0, 20, 0, 0)
            };

            string result = buttons[0];
            foreach (var buttonText in buttons)
            {
                var button = new Button
                {
                    Content = buttonText,
                    Margin = new Avalonia.Thickness(5),
                    Width = 80
                };
                button.Click += (s, e) =>
                {
                    result = buttonText;
                    dialog.Close();
                };
                buttonPanel.Children.Add(button);
            }

            panel.Children.Add(buttonPanel);
            dialog.Content = panel;

            await dialog.ShowDialog(owner);
            return result;
        }
    }
}
