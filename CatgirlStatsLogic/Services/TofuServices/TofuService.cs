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
    public class TofuNFTService : ITofuNFTService
    {
        public readonly Secrets _secrets;
        public readonly HttpClient _http;
        public TofuNFTService(
            Secrets secrets
        )
        {
            _secrets = secrets;
            _http = new HttpClient();
        }

        public async Task<IEnumerable<NFTListingModel>> GetTofuNFTs() 
        {
            var model = new 
            {
                filters = new { attributes = new {}, contracts = new[] {253} },
                attributes = new {},
                contracts = new[] {253},
                limit = 1000,
                offset = 0
            };
            var myContent = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            _http.DefaultRequestHeaders.Add("authority", "tofunft.com");         
            _http.DefaultRequestHeaders.Add("x-api-key", "Wky4c98XSWPdnbeH5q2SShYQzG3UFM2x");         
            _http.DefaultRequestHeaders.Add("referer", "https://tofunft.com/collection/catgirl-nft/items");         
            var response = await _http.PostAsync("https://tofunft-api.com/api/searchOrders", byteContent);
            var json = response.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<ListingResponseModel>(json);
            
            return res.Right.Data.Select(x => new NFTListingModel{
                ContractAddress = x.NFT_Contract.Contract,
                ContractName = x.NFT_Contract.Name, 
                Price = x.Price,
                TokenID = x.NFT.Token_Id,
                Type = 2,
                SellType = x.Type,
                ListedAt = x.Created_At,
                Owner = x.NFT.Owner,
                IsBundle = x.Is_Bundle
            });
        }
    }
}