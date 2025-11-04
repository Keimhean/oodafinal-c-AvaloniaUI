using CoffeeShopManagement.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeShopManagement.ViewModels
{
    public partial class ProductViewModel : ObservableObject
    {
        [ObservableProperty]
        private Product _product = new();

        [ObservableProperty]
        private bool _isEditing;

        public ProductViewModel(Product? product = null)
        {
            if (product != null)
            {
                Product = product;
                IsEditing = true;
            }
        }
    }
}