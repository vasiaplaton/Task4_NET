using IT_4_8.Models;
using MvvmHelpers.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IT_4_8.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        private int _quantityForPurchase;
        public int QuantityForPurchase
        {
            get => _quantityForPurchase;
            set
            {
                _quantityForPurchase = value;
                OnPropertyChanged(nameof(QuantityForPurchase));
            }
        }

        private string _newProductName;
        public string NewProductName
        {
            get => _newProductName;
            set
            {
                _newProductName = value;
                OnPropertyChanged(nameof(NewProductName));
            }
        }

        private int _newProductQuantity;
        public int NewProductQuantity
        {
            get => _newProductQuantity;
            set
            {
                _newProductQuantity = value;
                OnPropertyChanged(nameof(NewProductQuantity));
            }
        }

        private int _quantityToAdd;
        public int QuantityToAdd
        {
            get => _quantityToAdd;
            set
            {
                _quantityToAdd = value;
                OnPropertyChanged(nameof(QuantityToAdd));
            }
        }

        public AsyncCommand<string> SellProductCommand { get; }
        public AsyncCommand AddProductCommand { get; }
        public AsyncCommand AddQuantityCommand { get; }

        public MainViewModel()
        {
            LoadProducts();
            SellProductCommand = new AsyncCommand<string>(ExecuteSellProductCommandAsync);
            AddProductCommand = new AsyncCommand(ExecuteAddProductCommandAsync);
            AddQuantityCommand = new AsyncCommand(ExecuteAddQuantityCommandAsync);
        }

        private async Task LoadProducts()
        {
            await Task.Run(() =>
            {
                Products = new ObservableCollection<Product>
                {
                    new Product("Товар 1", 10),
                    new Product("Товар 2", 20),
                    new Product("Товар 3", 30)
                };

                Console.WriteLine("Products loaded:");
                foreach (var product in Products)
                {
                    Console.WriteLine($"- {product.Name}: {product.Quantity}");
                }
            });
        }

        private DeliveryService deliveryService = new DeliveryService();
        public async Task SellProductAsync(string productName)
        {
            var product = Products.FirstOrDefault(p => p.Name == productName);
            if (product != null)
            {
                int quantityToSell = QuantityForPurchase > 0 ? QuantityForPurchase : 1;
                product.Quantity -= quantityToSell;
                Console.WriteLine($"Sold {productName}. New quantity: {product.Quantity}");



                await deliveryService.Deliver(product);
                OnPropertyChanged(nameof(SelectedProduct));
                OnPropertyChanged(nameof(Products));

                if (product.Quantity <= 0)
                {
                    Products.Remove(product);
                    MessageBox.Show($"{product.Name} закончился!");
                }
            }
        }

        public async Task ExecuteSellProductCommandAsync(string productName)
        {
            Console.WriteLine($"Executing command for product: {productName}");
            await SellProductAsync(productName);
        }

        public async Task ExecuteAddProductCommandAsync()
        {
            var newProduct = new Product(NewProductName, NewProductQuantity);
            Products.Add(newProduct);
            NewProductName = string.Empty;
            NewProductQuantity = 0;
            OnPropertyChanged(nameof(NewProductName));
            OnPropertyChanged(nameof(NewProductQuantity));
        }

        public async Task ExecuteAddQuantityCommandAsync()
        {
            if (SelectedProduct != null)
            {
                SelectedProduct.Quantity += QuantityToAdd;
                QuantityToAdd = 0;
                OnPropertyChanged(nameof(QuantityToAdd));
                OnPropertyChanged(nameof(SelectedProduct));
                OnPropertyChanged(nameof(Products));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
