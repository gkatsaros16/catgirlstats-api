using System.Collections.Generic;
using System.Threading.Tasks;
using CatgirlStatsLogic.Services;
using CatgirlStatsModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CatgirlStatsApi.Controllers
{
    [EnableCors("Enable")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class TofuNFTController : ControllerBase
    {
        private readonly ITofuNFTService _tofuNFTService;

        public TofuNFTController(ITofuNFTService tofuNFTService) 
        {
            _tofuNFTService = tofuNFTService;
        }
        [HttpGet]
        public async Task<IEnumerable<NFTListingModel>> GetTofuNFTs()
        {
            return await _tofuNFTService.GetTofuNFTs();
        }         

        [HttpGet]
        public async Task<IEnumerable<NFTradeSalesResponseModel>> GetTofuNFTSales()
        {
            return await _tofuNFTService.GetTofuNFTSales();
        }         
    }
}