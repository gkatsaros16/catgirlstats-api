using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatgirlStatsModels;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace CatgirlStatsLogic.Services
{
    public class CatgirlStatsConsumer : ICatgirlStatsConsumer
    {
        private readonly IGraphQLClient _client;
        public CatgirlStatsConsumer(IGraphQLClient client)
        {
            _client = client;
        }

        public async Task<CatgirlModel> GetCatgirl(string hexId)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query GetCatgirls($where: Catgirl_filter) {
                    catgirls(
                        where: $where
                    ) {
                        id
                        characterId
                        season
                        rarity
                        nyaScore
                        __typename
                    }
                }",
                Variables = new {
                    where = new {
                        id = hexId,
                    }
                }
            };

            var response = await _client.SendQueryAsync<CatgirlsModel>(query);
            return response.Data.Catgirls.FirstOrDefault();
        }

        public async Task<CatgirlCharacterCount> GetCatgirlCount()
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query GetCount($id: String) {
                    characterCount(id: $id) {
                        id
                        total
                        __typename
                    }
                }",
                Variables = new {
                        id = "0:0",
                    }
            };

            var response = await _client.SendQueryAsync<GraphQLCatgirlCharacterCountReturn>(query);
            return response.Data.CharacterCount;
        }
    }
}