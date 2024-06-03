namespace IT_4_8.Models
{
    public class Customer
    {
        public string Name { get; set; }

        public Customer(string name)
        {
            Name = name;
        }

        public void BuyProduct(Store store, string productName)
        {
            store.SellProduct(productName);
        }
    }
}
