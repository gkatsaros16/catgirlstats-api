using System.Net.Http;
using System.Threading.Tasks;
using CatgirlStatsModels;
using MySql.Data.MySqlClient;

namespace CatgirlStatsLogic.Services
{
    public class SupportService : ISupportService
    {
        public readonly Secrets _secrets;
        public readonly HttpClient _http;
        public SupportService(
            Secrets secrets
        )
        {
            _secrets = secrets;
            _http = new HttpClient();
        }

        public async Task<int> SendMessage(string message) 
        {
            using (MySqlConnection conn = new MySqlConnection($"server=127.0.0.1;user=root;database=catgirl_stats;port=3306;password={_secrets.CatgirlStatsDBPass}"))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = 
                @"INSERT INTO messages
                (
                    message
                ) 
                VALUES
                (
                    @message 
                )";
                cmd.Parameters.AddWithValue("@message", message);
                var res = await Task.Run(() => cmd.ExecuteNonQuery());
                conn.Close();
                return res;
            }
        }
    }
}