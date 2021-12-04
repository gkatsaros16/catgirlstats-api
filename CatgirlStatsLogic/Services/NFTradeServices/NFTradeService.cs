using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CatgirlStatsModels;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace CatgirlStatsLogic.Services
{
    public class NFTradeService : INFTradeService
    {
        public readonly Secrets _secrets;
        public readonly HttpClient _http;
        public NFTradeService(
            Secrets secrets
        )
        {
            _secrets = secrets;
            _http = new HttpClient();
        }

        public async Task<IEnumerable<NFTradeListingResponseModel>> GetNFTradeListing() 
        {        
            var response = await _http.GetAsync("https://api.nftrade.com/api/v1/tokens?limit=800&skip=0&search=catgirl&order=&verified=&sort=listed_desc");
            var json = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(json);
            var res = JsonConvert.DeserializeObject<IEnumerable<NFTradeListingResponseModel>>(json);
            return res;
        }

        public async Task<IEnumerable<NFTradeSalesResponseModel>> GetNFTradeSales() 
        {        
            var response = await _http.GetAsync("https://api.nftrade.com/api/v1/tokens?limit=1000&skip=0&search=catgirl&order=&verified=&sort=sold_desc");
            var json = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(json);
            var res = JsonConvert.DeserializeObject<IEnumerable<NFTradeSalesResponseModel>>(json);
            return res;
        }
    }
}