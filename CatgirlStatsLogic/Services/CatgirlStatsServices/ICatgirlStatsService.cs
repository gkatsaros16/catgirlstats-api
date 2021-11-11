using System.Collections.Generic;
using System.Threading.Tasks;
using CatgirlStatsModels;

namespace CatgirlStatsLogic.Services
{
    public interface ICatgirlStatsService
    {
        Task<string> HelloWorld();
        Task<CatgirlModel> GetCatgirlDetails();
        Task<CatgirlCharacterCount> GetCatgirlCount();
        Task<IEnumerable<CatgirlsDomain>> GetCatgirls();
        Task<int> GetMaxCatgirlIdDecimal();
        Task AddCatgirl(string hexId);
        Task<IEnumerable<CatgirlNyaScoreWithCount>> GetAllCatgirlsNyaScoreCount(CatgirlsDomain catgirl);
    }
}