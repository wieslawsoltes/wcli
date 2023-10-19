using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WasabiCli.Models.Info;

public class ListKeysInfo
{
    [JsonPropertyName("keys")]
    public List<KeyInfo>? Keys { get; set; }
}
