using System;

namespace CatgirlStatsModels
{
    public class ListingRequestModel
    {
        public ListingRequestFilterModel filters { get; set; }
        public object attributes { get; set; }
        public int[] contracts { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
    }
    public class ListingRequestFilterModel
    {
        public object attributes { get; set; }
        public int[] contracts { get; set; }
    }
}
