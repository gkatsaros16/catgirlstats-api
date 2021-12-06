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
    public class NFTradeController : ControllerBase
    {
        private readonly INFTradeService _NFTradeService;

        public NFTradeController(INFTradeService NFTradeService) 
        {
            _NFTradeService = NFTradeService;
        }
        [HttpGet]
        public async Task<IEnumerable<NFTradeListingResponseModel>> GetNFTradeListing()
        {
            return await _NFTradeService.GetNFTradeListing();
        }     

        [HttpGet]
        public async Task<IEnumerable<NFTradeSalesResponseModel>> GetNFTradeSales()
        {
            return await _NFTradeService.GetNFTradeSales();
        }    

        [HttpGet]
        public async Task<IEnumerable<NFTradeSalesResponseModel>> GetNFTradeSales500()
        {
            return await _NFTradeService.GetNFTradeSales500();
        }         
    }
}