
using System.Collections.Generic;
using System.Threading.Tasks;
using CatgirlStatsModels;

namespace CatgirlStatsLogic.Services
{
    public interface ICatgirlStatsConsumer
    {
        Task<CatgirlModel> GetCatgirl(string hexId);
        Task<CatgirlCharacterCount> GetCatgirlCount();
    }
}