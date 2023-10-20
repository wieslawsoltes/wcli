using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WasabiRpc.Models.App;

public class State
{
    [JsonPropertyName("serverPrefix")]
    public string? ServerPrefix { get; set; }

    [JsonPropertyName("batchMode")]
    public bool BatchMode { get; set; }

    [JsonPropertyName("wallets")]
    public List<string?>? Wallets { get; set; }

    [JsonPropertyName("selectedWallet")]
    public string? SelectedWallet { get; set; }

    [JsonPropertyName("batches")]
    public List<Batch>? Batches { get; set; }
}
