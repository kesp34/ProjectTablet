using Newtonsoft.Json;

public class Space
{
    [JsonProperty("Id")]
    public string Id { get; set; }

    [JsonProperty("DisplayName")]
    public string Label { get; set; }

    [JsonProperty("Sections")]
    public string[] Areas { get; set; }

    [JsonProperty("Type")]
    public string Category { get; set; }

    [JsonProperty("IsCooperation")]
    public bool SharedUsage { get; set; }

    [JsonProperty("BlocksRooms")]
    public string[] RestrictedSpaces { get; set; }

    [JsonProperty("Name")]
    public string Title { get; set; }

    [JsonProperty("Line")]
    public string Pathway { get; set; }
}
