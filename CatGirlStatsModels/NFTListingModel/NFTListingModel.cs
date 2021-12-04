using System;

namespace CatgirlStatsModels
{
    public class NFTListingModel
    {
        public string ContractAddress { get; set; }
        public string ContractName { get; set; }
        public string ListedAt { get; set; }
        public string Price { get; set; }
        public string TokenID { get; set; }
        public int? Type { get; set; }
        public string SellType { get; set; }
        public string Owner { get; set; }
        public bool? IsBundle { get; set; }
    }
}