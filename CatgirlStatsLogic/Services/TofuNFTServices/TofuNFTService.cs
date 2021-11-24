using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CatgirlStatsLogic.Services
{
    public class TofuNFTService : ITofuNFTService
    {
        public readonly HttpClient _http;
        public TofuNFTService()
        {
            _http = new HttpClient();
        }

        public async Task<dynamic> GetTofuNFTs() 
        {
            var model = new { 
                filters = new {attributes = new {}, contracts = new[] {253}},
                attributes = new {},
                contracts = new[] {253},
                limit = 500,
                offset =  0
            };
            // model.ins
            var myContent = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _http.PostAsync("https://tofunft.com/api/searchOrders", byteContent);
            var json = response.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<dynamic>(json);
            return res;
        }
    }
}