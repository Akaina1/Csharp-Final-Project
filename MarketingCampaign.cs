﻿namespace FinalProject
{
    public class MarketingCampaign
    {
        public int Id { get; set; }
        public string AdDetails { get; set; }
        public double Cost { get; set; }
        public int Views { get; set; }
        public int Clicks { get; set; }
        public int SalesFromAd { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
