using System.Collections.Generic;

namespace Trakx.Shrimpy.ApiClient
{
    public class ShrimpyApiConfiguration
    {
#nullable disable
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public List<string> FavouriteExchanges { get; set; }
#nullable restore
    }
}