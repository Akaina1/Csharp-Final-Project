﻿namespace FinalProject
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; } // in Dollars
        public DateOnly DueDate { get; set; }
    }
}
