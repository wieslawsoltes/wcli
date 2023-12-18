using System.Text.Json.Serialization;

namespace WasabiRpc.Models.Rpc.Methods;

public class StopRpcMethod : RpcMethod
{
    [JsonPropertyName("params")]
    public object? Params { get; set; }
}