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
    public class SupportController : ControllerBase
    {
        private readonly ISupportService _SupportService;

        public SupportController(ISupportService SupportService) 
        {
            _SupportService = SupportService;
        }
        [HttpPost]
        public async Task<int> SendMessage(SendMessageModel model)
        {
            return await _SupportService.SendMessage(model.Message);
        }         
    }
}