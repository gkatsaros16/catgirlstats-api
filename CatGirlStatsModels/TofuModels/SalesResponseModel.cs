using System;
using System.Collections.Generic;

namespace CatgirlStatsModels
{
    public class SalesResponseModel
    {
        public int? Code { get; set; }
        public IEnumerable<SalesResponseDataModel> Data { get; set; }
    }

    public class SalesResponseDataModel
    {
        public string Category { get; set; }
        public string Created_At { get; set; }
        public decimal? Price { get; set; }
        public SalesResponseNFTModel NFT { get; set; }

    }

    public class SalesResponseNFTModel
    {
        public int? Token_ID{ get; set; }
        public SalesResponseNFTMetaModel Meta { get; set; }
    }

    public class SalesResponseNFTMetaModel
    {
        public IEnumerable<SalesResponseNFTMetaAttributesModel> Attributes { get; set; }
    }

    public class SalesResponseNFTMetaAttributesModel
    {
        public string Display_Type { get; set; }
        public string Trait_Type { get; set; }
        public dynamic Value { get; set; }
    }
}
