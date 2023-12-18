using System.Text.Json.Serialization;

namespace WasabiRpc.Models.Params.Send;

public class Coin
{
    [JsonPropertyName("transactionid")]
    public string? TransactionId { get; set; }

    [JsonPropertyName("index")]
    public int Index { get; set; }
}
