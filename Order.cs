namespace FinalProject
{
    public class Order
    {
        int Id { get; set; }
        string SupplierName { get; set; }
        string ProductName { get; set; }
        double AmountPerUnit { get; set; }
        int Units { get; set; }
        double TotalCost { get; set; }
        enum Status { Shipped, Delayed, Cancelled, Delivered }
    }
}
