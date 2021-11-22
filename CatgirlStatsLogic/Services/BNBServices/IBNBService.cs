
using System.Collections.Generic;
using System.Threading.Tasks;
using CatgirlStatsModels;

namespace CatgirlStatsLogic.Services
{
    public interface IBNBService
    {
        Task<string> GetBNBPriceForDate(string date);
        Task<string> GetBNBCurrentPrice();
        Task<string> GetBNBPriceForToday();
        Task SetBNBCurrentPrice();
        Task SetBNBPreviousDayPrice();
        Task SetBNBHistoricalPrice(string average, string date);
    }
}