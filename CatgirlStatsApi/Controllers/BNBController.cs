using System.Threading.Tasks;
using CatgirlStatsLogic.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace BNBApi.Controllers
{
    [EnableCors("Enable")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class BNBController : ControllerBase
    {
        private readonly IBNBService _BNBService;

        public BNBController(
            IBNBService BNBService
        )
        {
            _BNBService = BNBService;
        }

        [HttpGet]
        public async Task<string> GetBNBPriceForDate(string date)
        {
            return await _BNBService.GetBNBPriceForDate(date);
        }

        [HttpGet]
        public async Task<string> GetBNBCurrentPrice()
        {
            return await _BNBService.GetBNBCurrentPrice();
        }

        [HttpGet]
        public async Task<string> GetBNBPriceForToday()
        {
            return await _BNBService.GetBNBPriceForToday();
        }
        
        [HttpGet]
        public async Task SetBNBCurrentPrice()
        {
            await _BNBService.SetBNBCurrentPrice();
        }

        [HttpGet]
        public async Task SetBNBPreviousDayPrice()
        {
            await _BNBService.SetBNBPreviousDayPrice();
        }
    }
}
