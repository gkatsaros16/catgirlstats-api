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
            var response = await _http.GetAsync("https://api.nftrade.com//api/v1/tokens?limit=100&skip=0&contracts[]=f1232121-86c8-42d4-99ec-95326647663e&chains[]=56&search=&sort=sold_desc");
            var json = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(json);
            var res = JsonConvert.DeserializeObject<IEnumerable<NFTradeSalesResponseModel>>(json);
            return res;
        }
        public async Task<IEnumerable<NFTradeSalesResponseModel>> GetNFTradeSales500() 
        {   
            var final = new List<NFTradeSalesResponseModel>();
            var skips = new[] {0, 100, 200, 300, 400};
            foreach (var skip in skips)
            {
                var response = new HttpResponseMessage();
                try {
                    response = await _http.GetAsync($"https://api.nftrade.com/api/v1/tokens?limit=100&skip={skip}&search=catgirl&order=&verified=&sort=sold_desc");
                } catch (Exception e) {
                    Console.WriteLine(e.InnerException);
                    Console.WriteLine(e.Message);
                }
                
                var json = response.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<IEnumerable<NFTradeSalesResponseModel>>(json);
                var list = res.ToList();
                final.AddRange(list);
            }

            _http.DefaultRequestHeaders.Add("authority", "tofunft.com");         
            _http.DefaultRequestHeaders.Add("x-api-key", "8csYTAMBq8fFmNNN2C0KXONb66ePLJ4v");         
            _http.DefaultRequestHeaders.Add("referer", "https://tofunft.com/collection/catgirl-nft/items");         
            var responseTofu = await _http.GetAsync("https://tofunft-api.com/xapi/activity/sales?contract=catgirl-nft&offset=0&limit=1000");
            var jsonTofu = responseTofu.Content.ReadAsStringAsync().Result;
            var resTofu = JsonConvert.DeserializeObject<SalesResponseModel>(jsonTofu);
            
            var tofu = resTofu.Data.Select(x => new NFTradeSalesResponseModel{
                TokenID = x.NFT.Token_ID.ToString(),
                Last_Sell = x.Price.ToString(),
                Last_Sell_At = x.Created_At,
                NyaScore = x.NFT.Meta.Attributes.SingleOrDefault(x => x.Trait_Type == "Nya Score").Value
            });

            final.AddRange(tofu);
            return final;
        }
    }
}