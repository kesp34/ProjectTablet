using Newtonsoft.Json;

public class Reservation
{
    [JsonProperty("Id")]
    public int Id { get; set; }

    [JsonProperty("TimeStart")]
    public DateTime StartTime { get; set; }

    [JsonProperty("TimeEnd")]
    public DateTime EndTime { get; set; }

    [JsonProperty("Topic")]
    public string Subject { get; set; }

    private UserDetails userDetails;

    [JsonProperty("FullName")]
    public string FullName { get; set; }

    [JsonProperty("Company")]
    public string Company { get; set; }

    public void AssignUserDetails()
    {
        this.FullName = this.userDetails.FullName;
        this.Company = this.userDetails.Company;
    }
}

public class UserDetails
{
    public string FullName { get; set; }
    public string Company { get; set; }
}
