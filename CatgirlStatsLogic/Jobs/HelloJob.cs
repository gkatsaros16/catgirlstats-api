using System;
using System.Threading.Tasks;
using CatgirlStatsLogic.Services;
using Quartz;

namespace CatgirlStatsLogic.Jobs
{
    public class HelloJob : IJob
    {
        public readonly BNBService _BNBService;
        public HelloJob(BNBService BNBService) 
        {
            _BNBService = BNBService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Greetings from HelloJob!");
            // await _BNBService.SetBNBCurrentPrice();
        }
    }
}

// using System;
// using System.Threading.Tasks;
// using CatgirlStatsLogic.Services;
// using Quartz;

// namespace CatgirlStatsLogic.Jobs
// {
//     public class HelloJob : IJob
//     {
//         public readonly BNBService _BNBService;
//         public HelloJob(BNBService BNBService) 
//         {
//             _BNBService = BNBService;
//         }
//         public async Task Execute(IJobExecutionContext context)
//         {
//             await Console.Out.WriteLineAsync("Greetings from HelloJob!");
//             await _BNBService.SetBNBCurrentPrice();
//         }
//     }
// }