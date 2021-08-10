using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface ICompanyRepository 
    {
        Task<List<CompanyDetails>> GetCompanyDetails(string companyId, string tenantId);
        Task<bool> CheckCompanyNameAvailability(string tenantId, string companyId, string companyName);
        Task<bool> CreateCompany(CompanyRequest companyRequest);
        string GenerateUUID();
        Task<bool> UpdateCompany(CompanyRequest companyRequest);
        Task<bool> DeleteCompany(DeleteRequest deleteRequest);
        Task<ConvinienceFeesDetails> GetConvinienceFeesDetails(string companyId);
        Task<bool> UpdateConvinienceFees(ConvinienceFeesDetails convinienceFeesDetails);
    }
}