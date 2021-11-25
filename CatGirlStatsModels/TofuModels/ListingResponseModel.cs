using System;
using System.Collections.Generic;

namespace CatgirlStatsModels
{
    public class ListingResponseModel
    {
        public ListingResponse Right { get; set; }
    }

    public class ListingResponse
    {
        public IEnumerable<ListingResponseDataModel> Data { get; set; }
    }

    public class ListingResponseDataModel
    {
        public int? id { get; set; }
        public ListingResponseNFTModel NFT { get; set; }
        public ListingResponseNFTContractModel NFT_Contract { get; set; }
        public string Price { get; set; }
        public string Type { get; set; }
        public string End_At { get; set; }
    }

    public class ListingResponseNFTModel
    {
        public int? id { get; set; }
        public ListingResponseNFTMetaModel Meta { get; set; }
        public string NFT_Contract { get; set; }
        public string Token_Id { get; set; }
    }

    public class ListingResponseNFTMetaModel
    {
        public string Name { get; set; }
        public IEnumerable<ListingResponseNFTAttributesModel> Attributes { get; set; }
    }

    public class ListingResponseNFTAttributesModel
    {
        public dynamic Value { get; set; }
        public string Trait_Type { get; set; }
        public string Display_Type { get; set; }
    }

    public class ListingResponseNFTContractModel
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Contract { get; set; }
        public bool? Verified { get; set; }
        public int? Type { get; set; }
    }
}
