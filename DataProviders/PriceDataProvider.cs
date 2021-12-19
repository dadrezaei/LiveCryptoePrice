using CoinEx.Net.Interfaces;
using LiveCryptoePrice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveCryptoePrice.DataProviders
{
    public class PriceDataProvider
    {
        private readonly ICoinExSocketClient _binanceSocketClient;

        private readonly Dictionary<string, string> _cryptoes = new Dictionary<string, string>();

        public Action<PriceModel> OnPriceUpdate { get; set; }
        public bool IsSubscriptionStarted { get; private set; }

        public PriceDataProvider(ICoinExSocketClient binanceSocketClient)
        {
            _binanceSocketClient = binanceSocketClient;
            SetTickers();
        }

        private void SetTickers()
        {
            // Key: Exchange Ticker, Value: Symbol
            _cryptoes["BTCUSDC"] = "بیت کوین";
            _cryptoes["ETHUSDC"] = "اتریوم";
            _cryptoes["DOGEUSDC"] = "دوج کوین";
            _cryptoes["LTCUSDC"] = "لایت کوین";
            _cryptoes["ADAUSDC"] = "کاردانو";
            _cryptoes["XRPUSDC"] = "ریپل";
        }

        public async Task StartSubscriptions()
        {
            if (!IsSubscriptionStarted)
            {
                await SubscribeToBinance();
                IsSubscriptionStarted = true;
            }
        }

        private async Task SubscribeToBinance()
        {
            await _binanceSocketClient.SubscribeToSymbolStateUpdatesAsync(data =>
            {
                foreach (var tick in data.Data.Where(t => _cryptoes.ContainsKey(t.Symbol)))
                {
                    var symbol = tick.Symbol;
                    var persianName = _cryptoes.FirstOrDefault(x => x.Key == tick.Symbol).Value;
                    PriceModel price = new()
                    {
                        PersianName = persianName,
                        Symbol = symbol,
                        LastPrice = tick.Last,
                        Volume = tick.Volume, // USD volume
                        Open = tick.Open,
                        Close = tick.Close,
                        Period = tick.Period,
                        High = tick.High,
                        Low = tick.Low,
                        
                    };

                    OnPriceUpdate?.Invoke(price);
                }
            });
        }

        public async Task StopSubscriptions()
        {
            await _binanceSocketClient.UnsubscribeAllAsync();
            IsSubscriptionStarted = false;
        }
    }
}