using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using CatgirlStatsModels;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace CatgirlStatsLogic.Services
{
    public class BNBService : IBNBService
    {
        public readonly Secrets _secrets;
        public readonly HttpClient _http;
        public BNBService(
            Secrets secrets
        )
        {
            _secrets = secrets;
            _http = new HttpClient();
        }

        public async Task<string> GetBNBPriceForDate(string date) 
        {
            var format = date.Replace("\"", "");
            return DateTime.ParseExact(format, "M/d/yyyy", null) == DateTime.Today ? await GetBNBPriceForToday() : await GetBNBHistoricalPriceForDate(format);
        }

        public async Task<string> GetBNBPriceForToday() 
        {
            var price = "";
            using (MySqlConnection conn = new MySqlConnection($"server=127.0.0.1;user=root;database=catgirl_stats;port=3306;password={_secrets.CatgirlStatsDBPass}"))
            {               
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT price FROM `catgirls-stats`.bnbcurrentprice ORDER BY id DESC LIMIT 0, 1", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) 
                {
                    price = reader["Price"].ToString();
                }
                reader.Close();
            }
            return await Task.Run(() => price);
        }
        public async Task<string> GetBNBHistoricalPriceForDate(string date) 
        {
            var price = "";
            using (MySqlConnection conn = new MySqlConnection($"server=127.0.0.1;user=root;database=catgirl_stats;port=3306;password={_secrets.CatgirlStatsDBPass}"))
            {               
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Average FROM `catgirls-stats`.bnbprice where date = @date", conn);
                cmd.Parameters.AddWithValue("@date", date);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) 
                {
                    price = reader["Average"].ToString();
                }
                reader.Close();
            }
            return await Task.Run(() => price);
        }

        public async Task<string> GetBNBCurrentPrice()
        {
            var response = await _http.GetAsync("https://coinograph.io/ticker/?symbol=binance:bnbusdt");
            var json = response.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<dynamic>(json);
            return res.price;
        }

        public async Task SetBNBCurrentPrice()
        {
            var price = await GetBNBCurrentPrice();
            if (price == null) {
                price = await GetBNBCurrentPrice();
            }
            var date = DateTime.Today;
            var format = date.ToString("M/d/yyyy");

            using (MySqlConnection conn = new MySqlConnection($"server=127.0.0.1;user=root;database=catgirl_stats;port=3306;password={_secrets.CatgirlStatsDBPass}"))
            {               
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `catgirls-stats`.bnbcurrentprice (price, date) VALUES (@price, @date);", conn);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@date", format);
                await Task.Run(() => cmd.ExecuteNonQuery());
                conn.Close();
            }
        }

        public async Task SetBNBPreviousDayPrice()
        {
            var prices = new List<BNBPriceModel>();
            var date = DateTime.Today.AddDays(-1);
            var format = date.ToString("M/d/yyyy");
            var today = DateTime.Today;
            var tformat = date.ToString("M/d/yyyy");
            using (MySqlConnection conn = new MySqlConnection($"server=127.0.0.1;user=root;database=catgirl_stats;port=3306;password={_secrets.CatgirlStatsDBPass}"))
            {               
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(@"
                    SELECT
                        date, 
                        price
                    FROM 
                        bnbcurrentprice 
					WHERE
						date = @date
                    LIMIT 0, 100", conn);
                cmd.Parameters.AddWithValue("@date", format);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) 
                {
                    prices.Add(new BNBPriceModel {
                        Date = reader["date"].ToString(),
                        Price = reader["price"].ToString()
                    });                 
                }
                reader.Close();
                conn.Close();
            }
            var sum = 0.0;
            prices.ForEach(x => {
                sum += float.Parse(x.Price);
            });
            var average = sum / prices.Count;

            await SetBNBHistoricalPrice(average.ToString(), tformat);
        }

        public async Task SetBNBHistoricalPrice(string average, string date)
        {
            using (MySqlConnection conn = new MySqlConnection($"server=127.0.0.1;user=root;database=catgirl_stats;port=3306;password={_secrets.CatgirlStatsDBPass}"))
            {               
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `catgirls-stats`.bnbprice (average, date) VALUES (@average, @date);", conn);
                cmd.Parameters.AddWithValue("@average", average);
                cmd.Parameters.AddWithValue("@date", date);
                await Task.Run(() => cmd.ExecuteNonQuery());
                conn.Close();
            }
        }
    }
}