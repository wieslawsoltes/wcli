using System.Text.Json.Serialization;

namespace WasabiCli.Models.Results;

public class RpcMethod : Rpc
{
    [JsonPropertyName("method")]
    [JsonRequired]
    public string? Method { get; set; }

    [JsonPropertyName("params")]
    public object? Params { get; set; }
}
