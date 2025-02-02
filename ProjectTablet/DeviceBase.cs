public class DeviceBase : ComponentBase
{
    [ParameterAttribute]
    public String Identifier { get; set; }

    [Inject]
    public HttpClient ApiClient { get; set; }

    [Inject]
    public IJSRuntime JavaScriptRuntime { get; set; }

    public IEnumerable<Reservation>? Schedule { get; set; }

    public IEnumerable<Reservation> FilteredReservations { get; set; }

    public Room Space { get; set; }

    protected PeriodicTimer refreshTimer { get; set; }

    public DateTime CurrentDate { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await FetchSchedule();
        refreshTimer = new PeriodicTimer(TimeSpan.FromSeconds(60));
        StartTimer(refreshTimer);
    }

    private async void StartTimer(PeriodicTimer timer)
    {
        while (await timer.WaitForNextTickAsync())
        {
            Schedule = await FetchReservationsByDate();
            StateHasChanged();
        }
    }

    protected async Task FetchSchedule()
    {
        Schedule = null;
        try
        {
            space = await ApiClient.GetFromJsonAsync<Room>($"api/v2/space?spaceid={Id}");
            Schedule = await FetchReservationsByDate();
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }

    protected async Task<IEnumerable<Booking>>? FetchReservationsByDate()
    {
        String filterIDs;

        if (space.BlocksRooms.Count() == 0)
        {
            filterIDs = $"{space.Id}";
        }
        else
        {
            filterIDs = $"{String.Join(", ", space.BlocksRooms)}", "{space.Id}";
        }

        var reservations = await ApiClient.GetFromJsonAsync<IEnumerable<Booking>>($"api/v2/reservations?spaceids=[\"{filterIDs}\"]");
        var sortedReservations = reservations.OrderBy(booking => booking.Start);
        return sortedReservations;
    }
}
