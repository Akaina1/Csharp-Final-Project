using System.Diagnostics;

namespace FinalProject
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; } // in Dollars
        public double Cost { get; set; } // in Dollars
        public int InStock { get; set; }
        public int SoldLastMonth { get; set; }
        public int SoldThisMonth { get; set; }
        public int OnOrder { get; set; }
    }
}