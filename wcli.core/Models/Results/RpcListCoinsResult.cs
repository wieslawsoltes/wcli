using System.Collections.Generic;
using System.Text.Json.Serialization;
using WasabiCli.Models.Info;

namespace WasabiCli.Models.Results;

public class RpcListCoinsResult : Rpc
{
    [JsonPropertyName("result")]
    [JsonRequired]
    public List<CoinInfo>? Result { get; set; }
}
