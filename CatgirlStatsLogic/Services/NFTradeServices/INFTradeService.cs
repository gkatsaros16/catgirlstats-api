using System.Collections.Generic;
using System.Threading.Tasks;
using CatgirlStatsModels;

namespace CatgirlStatsLogic.Services
{
    public interface INFTradeService
    {
        Task<IEnumerable<NFTradeListingResponseModel>> GetNFTradeListing();
        Task<IEnumerable<NFTradeSalesResponseModel>> GetNFTradeSales();
        Task<IEnumerable<NFTradeSalesResponseModel>> GetNFTradeSales500();
    }
}