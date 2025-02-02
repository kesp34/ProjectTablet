public class SpaceBase : ComponentBase
{
    [Inject]
    public HttpClient Http { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Inject]
    NavigationManager NavManager { get; set; }

    public List<Reservation> RoomPlan { get; set; }

    [CascadingParameter]
    public CultureInfo Culture { get; set; }
    public int PlanCount { get; set; }
    
    private Timer timer;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync(DateTime);
        await GetRoomPlan();
        StartAnimation();
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            JSRuntime.InvokeVoidAsync("showTime", "de-DE");
        }

        return base.OnAfterRenderAsync(firstRender);
    }

    protected async Task GetRoomPlan()
    {
        var date = DateTime.Now.ToString("yyyy-MM-dd");

        var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("date", out var overrideDate))
        {
            date = overrideDate;
        }

        try
        {
            var planresult = await Http.GetFromJsonAsync<IEnumerable<Booking>>($"api/room/{date}");
            if (planresult != null)
            {
                RoomPlan = planresult
                .Where(booking => booking.TimeEnd.ToLocalTime().AddMinutes(30) > DateTime.Now)
                .Where(booking => !booking.Private)
                .Take(13)
                .ToList();
                PlanCount = RoomPlan.Count;

                await InvokeAsync(() =>
                {
                    StateHasChanged();
                });
            }
        }
        catch (Exception e) { Console.WriteLine(e); }
    }

    protected void StartAnimation()
    {
        timer = new Timer(new TimerCallback(async (_) =>
        {
            try
            {
                await GetRoomPlan();
                await JSRuntime.InvokeVoidAsync("");
                await JSRuntime.InvokeVoidAsync("");
            }
            catch (Exception) {}

        }), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
    }
}
