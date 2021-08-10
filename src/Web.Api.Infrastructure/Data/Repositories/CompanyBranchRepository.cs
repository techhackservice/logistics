using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using Dapper;
using Web.Api.Core.Enums;
using Newtonsoft.Json;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Infrastructure.Data.Repositories
{
    internal sealed class CompanyBranchRepository : ICompanyBranchRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IPIIServiceRepository _piiServiceRepository;
        public CompanyBranchRepository(IPIIServiceRepository piiServiceRepository, AppDbContext appDbContext)
        {
            _piiServiceRepository = piiServiceRepository;
            _appDbContext = appDbContext;
        }
        public async Task<List<CompanyBranchDetails>> GetCompanyBranchDetails(string branchId, string companyId)
        {
            List<CompanyBranchDetails> retCompanyBranchDetailsList = new List<CompanyBranchDetails>();

            try
            {
                using (var connection = _appDbContext.Connection)
                {
                    var tableName = $"Logisticmate.company_obj co, " + 
                                    $"Logisticmate.company_branch_obj cbo";

                    var ColumAssign = $"cbo.branch_id as BranchId, cbo.branch_name as BranchName, " + 
                                      $"cbo.company_id as CompanyId, co.company_name as CompanyName, " + 
                                      $"cbo.branch_url as BranchUrl, cbo.default_branch as DefaultBranch, " +
                                      $"cbo.license_no as LicenseNumber, cbo.status as Status, " +
                                      $"cbo.address_id as AddressId, cbo.contact_id as ContactId, " +
                                      $"cbo.created_by as CreatedBy, cbo.modified_by as ModifiedBy";

                    var selQuery = $"select " + ColumAssign + " from " + tableName;

                    var whereCondi = " where cbo.company_id = co.company_id";

                    if(!string.IsNullOrEmpty(companyId))
                        whereCondi += " and cbo.company_id = '" + companyId + "'";

                    if(!string.IsNullOrEmpty(branchId))
                        whereCondi += " and cbo.branch_id = '" + branchId + "'";

                    var activeCond = $" and cbo.status = '" + Status.Active.ToString() + "'" +
                                     $" and co.status = '" + Status.Active.ToString() + "'";

                    var orderCond = $" order by cbo.modified_by ASC ";

                    var sqlSelQuery = selQuery + whereCondi + activeCond + orderCond;
                    var sql = await connection.QueryAsync<CompanyBranchDetails>(sqlSelQuery);
                    retCompanyBranchDetailsList = sql.ToList();

                    AddressContactDetails addressContactDetails = new AddressContactDetails();
                    foreach (CompanyBranchDetails singleCompanyBranchDetails in retCompanyBranchDetailsList)
                    {
                        addressContactDetails.contactDetails = new ContactDetailsRequest();
                        addressContactDetails.addressDetails = new AddressDetailsRequest();
                        addressContactDetails.contactDetails.ContactId = singleCompanyBranchDetails.ContactId;
                        addressContactDetails.addressDetails.AddressId = singleCompanyBranchDetails.AddressId;
                        if (await _piiServiceRepository.GetContactAddressId(addressContactDetails))
                        {
                            singleCompanyBranchDetails.ContactDetails = addressContactDetails.contactDetails;
                            singleCompanyBranchDetails.AddressDetails = addressContactDetails.addressDetails;
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retCompanyBranchDetailsList;
        }

        public async Task<bool> CheckCompanyBranchNameAvailability(string branchId, string companyId, string branchName)
        {
            using (var connection = _appDbContext.Connection)
            {
                var tableName = $"company_branch_obj";
                var whereCond = $" where company_id = @CompanyId" +
                                $" and status = '" + Status.Active.ToString() + "'";

                if(!string.IsNullOrEmpty(branchName))
                    whereCond += $" and branch_name = @BranchName";

                if(!string.IsNullOrEmpty(branchId))
                    whereCond += $" and branch_id != @BranchId";

                object colValueParam = new
                {
                    BranchId = branchId,
                    CompanyId = companyId,
                    BranchName = branchName
                };
                var sqlSelQuery = $"select * from " + tableName + whereCond;
                var sqlResult = await connection.QueryAsync<string>(sqlSelQuery, colValueParam);
                return sqlResult.ToList().Any();
            }
        }

        public async Task<bool> CreateCompanyBranch(CompanyBranchRequest companyBranchRequest)
        {
            try
            {
                var uuid = GenerateUUID();
                bool sqlResult = true;
                companyBranchRequest.BranchId = uuid;

                var tableName = $"Logisticmate.company_branch_obj";

                var colName = $"branch_id, branch_name, branch_url, " +
                              $"company_id, default_branch, license_no, " +
                              $"address_id, contact_id, " +
                              $"status, created_by, created_on";

                var colValueName = $"@BranchId, @BranchName, @BranchUrl, " +
                                   $"@CompanyId, @DefaultBranch, @LicenseNumber, " +
                                   $"@AddressId, @ContactId, " +
                                   $"@Status, @CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                if(String.IsNullOrEmpty(companyBranchRequest.AddressId) || companyBranchRequest.AddressId == "string")
                {
                    AddressContactDetails addressContactDetails = new AddressContactDetails();
                    if(companyBranchRequest.AddressDetails == null)
                        companyBranchRequest.AddressDetails = new AddressDetailsRequest();

                    if(companyBranchRequest.ContactDetails == null)
                        companyBranchRequest.ContactDetails = new ContactDetailsRequest();

                    addressContactDetails.addressDetails = companyBranchRequest.AddressDetails;
                    addressContactDetails.contactDetails = companyBranchRequest.ContactDetails;
                    sqlResult = await _piiServiceRepository.CreateContactAddressId(addressContactDetails);
                    companyBranchRequest.AddressId = companyBranchRequest.AddressDetails.AddressId;
                    companyBranchRequest.ContactId = companyBranchRequest.ContactDetails.ContactId;
                }

                object colValueParam = new
                {
                    BranchId = companyBranchRequest.BranchId,
                    BranchName = companyBranchRequest.BranchName,
                    CompanyId = companyBranchRequest.CompanyId,
                    BranchUrl = companyBranchRequest.BranchUrl,
                    DefaultBranch = companyBranchRequest.DefaultBranch,
                    LicenseNumber = companyBranchRequest.LicenseNumber,
                    AddressId = companyBranchRequest.AddressId,
                    ContactId =  companyBranchRequest.ContactId,
                    Status = Status.Active.ToString(),
                    CreatedBy = companyBranchRequest.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };

                if(sqlResult)
                {
                    using (var connection = _appDbContext.Connection)
                    {
                        sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlInsQuery, colValueParam));
                    }
                }
                return sqlResult;
            }
            catch (Exception Err)
            {
                companyBranchRequest.ErrorMsg = Err.Message.ToString();
                return false;
            }
        }

        public string GenerateUUID()
        {
            using (var connection = _appDbContext.Connection)
            {
                var sqlQuery = $"Select UUID()";
                var sql = connection.Query<string>(sqlQuery);
                return sql.FirstOrDefault();
            }
        }
        public async Task<bool> UpdateCompanyBranch(CompanyBranchRequest companyBranchRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"Logisticmate.company_branch_obj";

                var colName = $"branch_id = @BranchId, branch_name = @BranchName, " +
                              $"branch_url = @BranchUrl, company_id = @CompanyId, " +
                              $"default_branch = @DefaultBranch, license_no = @LicenseNumber, " +
                              $"address_id = @AddressId, contact_id = @ContactId, " +
                              $"status = @Status, modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where branch_id = @BranchId";

                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                if(String.IsNullOrEmpty(companyBranchRequest.AddressId) || companyBranchRequest.AddressId == "string")
                {
                    AddressContactDetails addressContactDetails = new AddressContactDetails();
                    if(companyBranchRequest.AddressDetails == null)
                        companyBranchRequest.AddressDetails = new AddressDetailsRequest();

                    if(companyBranchRequest.ContactDetails == null)
                        companyBranchRequest.ContactDetails = new ContactDetailsRequest();

                    addressContactDetails.addressDetails = companyBranchRequest.AddressDetails;
                    addressContactDetails.contactDetails = companyBranchRequest.ContactDetails;
                    sqlResult = await _piiServiceRepository.CreateContactAddressId(addressContactDetails);
                    companyBranchRequest.AddressId = companyBranchRequest.AddressDetails.AddressId;
                    companyBranchRequest.ContactId = companyBranchRequest.ContactDetails.ContactId;
                }
                else if(companyBranchRequest.ContactDetails != null && companyBranchRequest.AddressDetails != null)
                {
                    if(String.IsNullOrEmpty(companyBranchRequest.DefaultBranch) 
                    && companyBranchRequest.DefaultBranch == "no")
                    {
                        AddressContactDetails addressContactDetails = new AddressContactDetails();
                        addressContactDetails.addressDetails = companyBranchRequest.AddressDetails;
                        addressContactDetails.contactDetails = companyBranchRequest.ContactDetails;
                        sqlResult = await _piiServiceRepository.UpdateContactAddressId(addressContactDetails);
                    }
                }

                if(String.IsNullOrEmpty(companyBranchRequest.BranchId) || companyBranchRequest.BranchId == "string")
                    companyBranchRequest.BranchId = await GetCompanyBranchId(companyBranchRequest.CompanyId);

                object colValueParam = new
                {
                    BranchId = companyBranchRequest.BranchId,
                    BranchName = companyBranchRequest.BranchName,
                    CompanyId = companyBranchRequest.CompanyId,
                    BranchUrl = companyBranchRequest.BranchUrl,
                    DefaultBranch = companyBranchRequest.DefaultBranch,
                    LicenseNumber = companyBranchRequest.LicenseNumber,
                    AddressId = companyBranchRequest.AddressId,
                    ContactId =  companyBranchRequest.ContactId,
                    Status = Status.Active.ToString(),
                    ModifiedBy = companyBranchRequest.ModifiedBy,
                    ModifiedOn = DateTime.UtcNow
                };

                if(sqlResult)
                {
                    using (var connection = _appDbContext.Connection)
                    {
                        sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlUpdateQuery, colValueParam));
                    }
                }
                return sqlResult;
            }
            catch (Exception Err)
            {
                companyBranchRequest.ErrorMsg = Err.Message.ToString();
                return false;
            }
        }
        async Task<string> GetCompanyBranchId(string companyId)
        {
            try
            {
                var tableName = $"Logisticmate.company_branch_obj";

                var whereCond = $" where company_id = '" + companyId + "'" +
                                $" and default_branch = 'yes'";
                var sqlSelectQuery = $"select branch_id from "+ tableName + whereCond;

                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = await connection.QueryAsync<string>(sqlSelectQuery);
                    return sqlResult.FirstOrDefault();
                }
            }
            catch (Exception Err)
            {
                var ErrorMsg = Err.Message.ToString();
                return "";
            }
        }
        public async Task<bool> DeleteCompanyBranch(DeleteRequest request)
        {
            try
            {
                var tableName = $"Logisticmate.company_branch_obj";
                var colName = $"status = @Status, modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where branch_id = @BranchId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    BranchId = request.Id,
                    Status = Status.Inactive.ToString(),
                    ModifiedBy = request.DeletedBy,
                    ModifiedOn = DateTime.UtcNow
                };
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = await connection.ExecuteAsync(sqlUpdateQuery, colValueParam);
                    return true;
                }
            }
            catch (Exception Err)
            {
                request.ErrorMsg = Err.Message.ToString();
                return false;
            }
        }

    }
}