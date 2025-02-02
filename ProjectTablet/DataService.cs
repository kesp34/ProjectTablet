public class DataService : IDataService
{
    private readonly IConfiguration config;

    public DataService(IConfiguration config)
    {
        this.config = config;
    }

    public async Task<Space> RetrieveSpace(string id)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "ApiKey aaa-bbbb-cccc");

            var space = await client.GetFromJsonAsync<Space>($"{id}");
            return space;
        }
    }

    public async Task<IEnumerable<Reservation>>? RetrieveReservations(IEnumerable<string> spaceIDs, DateTimeOffset date)
    {
        string formattedDate = date.ToString("s").Split("T")[0];
        using (var client = new HttpClient())
        {
            IEnumerable<Reservation>? reservations;
            client.DefaultRequestHeaders.Add("Authorization", "ApiKey aaa-bbbb-cccccc");
            Uri baseUri = new Uri("");
            Uri uri;

            if (spaceIDs.Count() == 1)
            {
                uri = new Uri(baseUri, $"reservations/v2?filter=Space/Id eq '{spaceIDs.FirstOrDefault()}' and Date eq {formattedDate} and Status ne 'Unconfirmed'");
            }
            else
            {
                string spacesString = string.Join("', '", spaceIDs);
                uri = new Uri(baseUri, $"reservations/v2?filter=Space/Id in ('{spacesString}') and Date eq {formattedDate} and Status ne 'Unconfirmed'");
            }

            try
            {
                reservations = await client.GetFromJsonAsync<IEnumerable<Reservation>>(uri.AbsoluteUri);
                foreach (Reservation reservation in reservations)
                {
                    reservation.AssignUserDetails();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return reservations;
        }
    }

    public async Task<IEnumerable<Reservation>> RetrieveReservations(IEnumerable<string> spaceIDs)
    {
        DateTimeOffset date = DateTime.UtcNow;
        var reservations = await RetrieveReservations(spaceIDs, date);
        return reservations;
    }
}