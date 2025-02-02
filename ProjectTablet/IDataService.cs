public interface IDataService
{
    Task<Space> RetrieveSpace(string identifier);
    Task<IEnumerable<Reservation>>? RetrieveReservations(IEnumerable<string> spaceIDs, DateTimeOffset date);
    Task<IEnumerable<Reservation>> RetrieveReservations(IEnumerable<string> spaceIDs);
}
