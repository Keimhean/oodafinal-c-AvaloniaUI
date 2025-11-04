using Avalonia.Controls;
using Avalonia.Media;

namespace CoffeeShopManagement.Views.Controls
{
    public partial class NotificationControl : UserControl
    {
    public string Message { get; set; } = string.Empty;

        public NotificationControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public NotificationControl(string message, Color color) : this()
        {
            Message = message;
            
            if (this.FindControl<Border>("Border") is Border border)
            {
                border.Background = new SolidColorBrush(color);
            }
        }
    }
}