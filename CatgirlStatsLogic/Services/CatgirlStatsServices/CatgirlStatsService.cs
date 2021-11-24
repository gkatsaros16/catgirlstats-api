using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CatgirlStatsModels;
using MySql.Data.MySqlClient;
using System.Linq;

namespace CatgirlStatsLogic.Services
{
    public class CatgirlStatsService : ICatgirlStatsService
    {
        public readonly ICatgirlStatsConsumer _consumer;
        public readonly Secrets _secrets;
        public CatgirlStatsService(
            ICatgirlStatsConsumer consumer,
            Secrets secrets
        ) 
        {
            _consumer = consumer;
            _secrets = secrets;
        }
        public async Task<string> HelloWorld() {
            return await Task.Run(() => "Hello World");
        }

        public async Task<CatgirlModel> GetCatgirlDetails() 
        {
            return await _consumer.GetCatgirl("0x0");
        }

        public async Task<CatgirlCharacterCount> GetCatgirlCount() 
        {
            return await _consumer.GetCatgirlCount();
        }

        public async Task<IEnumerable<CatgirlsDomain>> GetCatgirls() 
        {
            var catgirls = new List<CatgirlsDomain>();
            using (MySqlConnection conn = new MySqlConnection($"server=127.0.0.1;user=root;database=catgirl_stats;port=3306;password={_secrets.CatgirlStatsDBPass}"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from catgirls LIMIT 0, 500000", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) 
                {
                    catgirls.Add(new CatgirlsDomain {
                        id = Convert.ToInt32(reader["id"]),
                        Catgirl__Typename = reader["Catgirl__Typename"].ToString(),
                        CatgirlCharacterId = reader["CatgirlCharacterId"].ToString(),
                        CatgirlIdHex = reader["CatgirlIdHex"].ToString(),
                        CatgirlIdDecimal = reader["CatgirlIdDecimal"].ToString(),
                        CatgirlNyaScore = reader["CatgirlNyaScore"].ToString(),
                        CatgirlRarity = reader["CatgirlRarity"].ToString(),
                        CatgirlSeason = reader["CatgirlSeason"].ToString()
                    });                   
                }
                reader.Close();
            }
            return await Task.Run(() => catgirls);
        }

        public async Task<int> GetMaxCatgirlIdDecimal() 
        {
            var maxCatgirlId = 0;
            using (MySqlConnection conn = new MySqlConnection($"server=127.0.0.1;user=root;database=catgirl_stats;port=3306;password={_secrets.CatgirlStatsDBPass}"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT CatgirlIdDecimal from catgirls LIMIT 350000, 500000", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) 
                {
                    maxCatgirlId = Convert.ToInt32(reader["CatgirlIdDecimal"]);              
                }
                reader.Close();
            }
            return await Task.Run(() => maxCatgirlId);
        }

        public async Task<IEnumerable<CatgirlNyaScoreWithCount>> GetAllCatgirlsNyaScoreCount(CatgirlsDomain catgirl) 
        {
            var catgirls = new List<CatgirlNyaScoreWithCount>();
            using (MySqlConnection conn = new MySqlConnection($"server=127.0.0.1;user=root;database=catgirl_stats;port=3306;password={_secrets.CatgirlStatsDBPass}"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(@"
                    SELECT
                        CatgirlNyaScore, 
                        Count(*) as Count
                    FROM 
                        catgirls 
					WHERE
						catgirlCharacterID = @catgirlCharacterID AND catgirlRarity = @catgirlRarity
                    GROUP BY 
                        CatgirlNyaScore
                    ORDER BY count(*) DESC", conn);
                cmd.Parameters.AddWithValue("@catgirlCharacterID", catgirl.CatgirlCharacterId);
                cmd.Parameters.AddWithValue("@catgirlRarity", catgirl.CatgirlRarity);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) 
                {
                    catgirls.Add(new CatgirlNyaScoreWithCount {
                        Count = Convert.ToInt32(reader["Count"]),
                        CatgirlNyaScore = reader["CatgirlNyaScore"].ToString(),
                    });                 
                }
                reader.Close();
            }
            return await Task.Run(() => catgirls);
        }

        public async Task AddCatgirl(string hexId) 
        {
            var catgirl = await _consumer.GetCatgirl(hexId);
            var catgirlsDomain = new CatgirlsDomain {
                Catgirl__Typename = catgirl.__Typename,
                CatgirlCharacterId = catgirl.CharacterId,
                CatgirlIdDecimal = new System.ComponentModel.Int32Converter().ConvertFromString(catgirl.id).ToString(),
                CatgirlIdHex = catgirl.id,
                CatgirlNyaScore = catgirl.NyaScore,
                CatgirlRarity = catgirl.Rarity,
                CatgirlSeason = catgirl.Season
            };
            using (MySqlConnection conn = new MySqlConnection($"server=127.0.0.1;user=root;database=catgirl_stats;port=3306;password={_secrets.CatgirlStatsDBPass}"))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = 
                @"INSERT INTO catgirls
                (
                    Catgirl__Typename, 
                    CatgirlCharacterId, 
                    CatgirlIdHex, 
                    CatgirlIdDecimal, 
                    CatgirlNyaScore, 
                    CatgirlRarity, 
                    CatgirlSeason
                ) 
                VALUES
                (
                    @catgirl__Typename, 
                    @catgirlCharacterId, 
                    @catgirlIdHex, 
                    @catgirlIdDecimal, 
                    @catgirlNyaScore, 
                    @catgirlRarity, 
                    @catgirlSeason
                )";
                cmd.Parameters.AddWithValue("@catgirl__Typename", catgirlsDomain.Catgirl__Typename);
                cmd.Parameters.AddWithValue("@catgirlCharacterId", catgirlsDomain.CatgirlCharacterId);
                cmd.Parameters.AddWithValue("@catgirlIdHex", catgirlsDomain.CatgirlIdHex);
                cmd.Parameters.AddWithValue("@catgirlIdDecimal", catgirlsDomain.CatgirlIdDecimal);
                cmd.Parameters.AddWithValue("@catgirlNyaScore", catgirlsDomain.CatgirlNyaScore);
                cmd.Parameters.AddWithValue("@catgirlRarity", catgirlsDomain.CatgirlRarity);
                cmd.Parameters.AddWithValue("@catgirlSeason", catgirlsDomain.CatgirlSeason);
                await  Task.Run(() => cmd.ExecuteNonQuery());
                conn.Close();
            }
        }
    }
}
