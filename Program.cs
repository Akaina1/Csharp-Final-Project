using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    internal class Program
    {
        static void Main()
        {
            // test inventory manager method
            InventoryManager inventoryManager = new(); // can just use new() instead of new InventoryManager() to instantiate a new object - C# 9.0 feature


            //test add to inventory method

            inventoryManager.AddToInventory(19051, "Apple", 1.99, 0.99, 70, 56);

            inventoryManager.ShowInventory();
        }
    }
}
