namespace SimpleApp.Entities
{
    public class SoItem
    {
        public int SoItemId { get; set; }
        public int SoOrderId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int Price { get; set; }

        public SoOrder Order { get; set; } = null!;
    }
}
