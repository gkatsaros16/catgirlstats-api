using System.Collections.Generic;
using System.Threading.Tasks;
using CatgirlStatsModels;

namespace CatgirlStatsLogic.Services
{
    public interface ITofuNFTService
    {
        Task<IEnumerable<NFTListingModel>> GetTofuNFTs();
        Task<IEnumerable<NFTradeSalesResponseModel>> GetTofuNFTSales();
    }
}