namespace LiveCryptoePrice.Models
{
    public static class DollarModel
    {
        private static int _dollarRate;
        public static int DollarRate
        {
            get
            {
                if (Counter == 60)
                {
                    var dlr = GetDollarRate();
                    DollarRate = dlr;
                    Counter = 0;
                    return _dollarRate;
                }
                else
                {
                    Counter++;
                    return _dollarRate;
                }
            }
            set => _dollarRate = value;
        }
        private static int Counter { set; get; } = 60;


        private static int GetDollarRate()
        {
            return 31234;
        }
    }
}
