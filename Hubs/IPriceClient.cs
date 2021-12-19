using LiveCryptoePrice.Models;
using System.Threading.Tasks;

namespace LiveCryptoePrice.Hubs
{
    public interface IPriceClient
    {
        Task UpdatePrice(PriceModel price);
    }
}
