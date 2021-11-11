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
    public class HealthCheck : ControllerBase
    {
        private readonly ICatgirlStatsService _catgirlStatsService;

        public HealthCheck(){}

        [HttpGet]
        public async Task<string> HelloWorld()
        {
            return await _catgirlStatsService.HelloWorld();
        }         
    }
}
