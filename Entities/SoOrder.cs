namespace SimpleApp.Entities
{
    public class SoOrder
    {
        public int SoOrderId { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public int ComCustomerId { get; set; }
        public string Address { get; set; } = string.Empty;
        public ComCustomer Customer { get; set; } = null!;
        public List<SoItem> Items { get; set; } = [];
    }
}
