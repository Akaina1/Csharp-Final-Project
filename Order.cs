namespace FinalProject
{
    public class Order
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string ProductName { get; set; }
        public double AmountPerUnit { get; set; }
        public int Units { get; set; }
        public double TotalCost { get; set; }
        public enum Status { Shipped, Delayed, Cancelled, Delivered, Pending }
        public Status OrderStatus { get; set; }

    }
}
