using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace IT_4_8.Models
{
    public class DeliveryService : IDeliveryService
    {

        public async Task Deliver(Product product)
        {
            await Task.Delay(2000);
            MessageBox.Show($"{product.Name} успешно доставлен.");
        }
    }
}
