//Endpoints

[ApiController]
[Route("api/v2")]
public class Controller : ControllerBase
{
    // Attribute

    private readonly IApiService serviceHandler;

    // Constructor
    public MainController(IApiService serviceHandler)
    {
        this.serviceHandler = serviceHandler;
    }
    // Methods
    public IApiService GetServiceHandler()
    {
        return serviceHandler;
    }

    private IEnumerable<String> ParseJsonList(String json)
    {
        IEnumerable<String> identifiers = JsonConvert.DeserializeObject<List<string>>(json);
        return identifiers;
    }

    [HttpGet("space")]
    public async Task<Room> GetSpace(string spaceID)
    {
        Room space = await serviceHandler.GetRoom(spaceID);
        return space;
    }

    [HttpGet("reservations")]
    public async Task<IEnumerable<Booking>> GetReservations(String spaceIDs, DateTimeOffset? referenceDate = null)
    {
        if (referenceDate == null)
        {
            IEnumerable<Booking> reservations = await serviceHandler.GetBookings(ParseJsonList(spaceIDs));
            return reservations;
        }
        else
        {
            IEnumerable<Booking> reservations = await serviceHandler.GetBookings(ParseJsonList(spaceIDs), (DateTimeOffset)referenceDate);
            return reservations;
        }
    }
}

