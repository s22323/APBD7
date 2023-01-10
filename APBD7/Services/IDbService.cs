using APBD7.DTOs.Requests;
using APBD7.DTOs.Responses;

namespace APBD7.Services
{
    public interface IDbService
    {
        Task<IEnumerable<GetTripsDTO>> GetTrips();
        Task DeleteClient(int id);
        Task AssignClientToTrip(int idTrip, AssignClientToTripDTO client);

    }
}
