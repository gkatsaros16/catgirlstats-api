using System.Threading.Tasks;
using CatgirlStatsLogic.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CatgirlStatsApi.Controllers
{
    [EnableCors("Enable")]
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
