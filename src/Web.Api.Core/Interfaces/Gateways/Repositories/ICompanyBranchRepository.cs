using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface ICompanyBranchRepository 
    {
        Task<List<CompanyBranchDetails>> GetCompanyBranchDetails(string branchId, string companyId);
        Task<bool> CheckCompanyBranchNameAvailability(string branchId, string companyId, string branchName);
        Task<bool> CreateCompanyBranch(CompanyBranchRequest companyBranchRequest);
        string GenerateUUID();
        Task<bool> UpdateCompanyBranch(CompanyBranchRequest companyBranchRequest);
        Task<bool> DeleteCompanyBranch(DeleteRequest deleteRequest);
    }
}