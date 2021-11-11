using System.Collections.Generic;

namespace CatgirlStatsModels
{
    public class CatgirlsModel
    {
        public IEnumerable<CatgirlModel> Catgirls { get; set; }
    }
    public class GraphQLCatgirlCharacterCountReturn
    {
        public CatgirlCharacterCount CharacterCount { get; set; }
    }
}
