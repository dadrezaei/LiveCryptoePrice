namespace LiveCryptoePrice.Models
{
    public class PriceModel
    {
        public decimal LastPriceIRR => LastPrice * DollarModel.DollarRate ;
        public string PersianName { get; set; }
        public string Symbol { get; set; }
        public decimal LastPrice { get; set; }
        public decimal Volume { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal Period { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Rank => 0;
        public string Key => $"{Symbol}:{Symbol}";

    }
}
