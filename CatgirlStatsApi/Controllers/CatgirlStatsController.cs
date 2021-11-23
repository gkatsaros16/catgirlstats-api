using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatgirlStatsLogic.Services;
using CatgirlStatsModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CatgirlStatsApi.Controllers
{
    [EnableCors("Enable")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class CatgirlStatsController : ControllerBase
    {
        private readonly ICatgirlStatsService _catgirlStatsService;

        public CatgirlStatsController(
            ICatgirlStatsService catgirlStatsService
        )
        {
            _catgirlStatsService = catgirlStatsService;
        }

        [HttpGet]
        public async Task<string> HelloWorld()
        {
            return await _catgirlStatsService.HelloWorld();
        }

        [HttpGet]
        public async Task<CatgirlModel> GetCatgirlDetails()
        {
            return await _catgirlStatsService.GetCatgirlDetails();
        }

        [HttpGet]
        public async Task<CatgirlCharacterCount> GetCatgirlCount()
        {
            return await _catgirlStatsService.GetCatgirlCount();
        }

        [HttpPost]
        public async Task AddCatgirl()
        {
            var lastAdded = await _catgirlStatsService.GetMaxCatgirlIdDecimal();
            for (int i = lastAdded + 1; i <= 500000; i++)
            {
                var hex = "0x" + i.ToString("X").ToLower();
                await _catgirlStatsService.AddCatgirl(hex);
            }   
        }

        [HttpGet]
        public async Task<string> GetMaxCatgirlIdDecimal()
        {
            try {
                var res = await _catgirlStatsService.GetMaxCatgirlIdDecimal();
                return res.ToString();
            } catch (Exception e) {
                return e.InnerException.Message;
            }
            
        }

        [HttpGet]
        public async Task<IEnumerable<CatgirlsDomain>> GetCatgirls()
        {
            return await _catgirlStatsService.GetCatgirls();
        }

        [HttpPost]
        public async Task<IEnumerable<CatgirlNyaScoreWithCount>> GetAllCatgirlsNyaScoreCount(CatgirlsDomain catgirl)
        {
            return await _catgirlStatsService.GetAllCatgirlsNyaScoreCount(catgirl);
        }
    }
}
