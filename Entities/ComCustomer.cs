namespace SimpleApp.Entities
{
    public class ComCustomer
    {
        public int ComCustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public List<SoOrder> Orders { get; set; } = [];
    }
}
