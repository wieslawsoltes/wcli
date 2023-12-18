using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WasabiRpc.Models.Info;

public class ListCoinsInfo
{
    [JsonPropertyName("coins")]
    public List<CoinInfo>? Coins { get; set; }
}
