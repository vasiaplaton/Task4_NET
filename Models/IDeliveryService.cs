using System.Threading.Tasks;

namespace IT_4_8.Models
{
    public interface IDeliveryService
    {
        Task Deliver(Product product);
    }
}
