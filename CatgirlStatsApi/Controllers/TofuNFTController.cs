using System.Collections.Generic;
using System.Threading.Tasks;
using CatgirlStatsLogic.Services;
using CatgirlStatsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CatgirlStatsApi.Controllers
{
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
        public async Task<string> GetTofuNFTs()
        {
            return await _tofuNFTService.GetTofuNFTs();
        }         
    }
}