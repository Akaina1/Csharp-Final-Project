namespace FinalProject
{
    public class SalesOrder
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public double SalePrice { get; set; } // in Dollars
        public enum Status { Pending, Delayed, Shipped, Delivered }
        public Status OrderStatus { get; set; }
    }
}
