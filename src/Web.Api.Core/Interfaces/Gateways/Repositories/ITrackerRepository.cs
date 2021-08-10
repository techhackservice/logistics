using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface ITrackerRepository 
    {
        Task<List<TrackerDetails>> GetTrackerDetails(string trackId, string email, string companyUrl);
    }
}