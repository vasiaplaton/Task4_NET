using System;
using System.Collections.Generic;

namespace IT_4_8.Models
{
    public class Store
    {
        public List<Product> products = new List<Product>();
        public event Action<string> ProductSoldEvent;

        public Store()
        {
            InitializeStore();
        }

        private void InitializeStore()
        {
        }

        public void AddProduct(Product product)
        {
            products.Add(product);
        }

        public async void SellProduct(string productName)
        {
            var product = products.Find(p => p.Name == productName);
            if (product != null)
            {
                product.Quantity--;
                if (product.Quantity <= 0)
                {
                    OnProductSoldEvent(productName);
                    var deliveryService = new DeliveryService();
                    await deliveryService.Deliver(product);
                }
            }
        }

        protected virtual void OnProductSoldEvent(string productName)
        {
            ProductSoldEvent?.Invoke(productName);
        }
    }
}
