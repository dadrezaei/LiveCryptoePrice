using LiveCryptoePrice.DataProviders;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace LiveCryptoePrice.Hubs
{
    public class PriceHub : Hub<IPriceClient>
    {
        private readonly PriceDataProvider _dataProvider;

        public PriceHub(PriceDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public override async Task OnConnectedAsync()
        {
            await _dataProvider.StartSubscriptions();
            var clients = Clients.All;
            _dataProvider.OnPriceUpdate = price =>
            {
                clients.UpdatePrice(price);
            };
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _dataProvider.StopSubscriptions();
            var clients = Clients.All;
            _dataProvider.OnPriceUpdate = price =>
            {
                clients.UpdatePrice(price);
            };
            await base.OnDisconnectedAsync(exception);
        }
    }
}
