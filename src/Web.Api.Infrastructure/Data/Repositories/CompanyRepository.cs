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
    internal sealed class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IPIIServiceRepository _piiServiceRepository;
        private readonly ICompanyBranchRepository _companyBranchRepository;
        public CompanyRepository(IPIIServiceRepository piiServiceRepository, ICompanyBranchRepository companyBranchRepository, AppDbContext appDbContext)
        {
            _piiServiceRepository = piiServiceRepository;
            _companyBranchRepository = companyBranchRepository;
            _appDbContext = appDbContext;
        }
        public async Task<List<CompanyDetails>> GetCompanyDetails(string companyId, string tenantId)
        {
            List<CompanyDetails> retCompanyDetailsList = new List<CompanyDetails>();

            try
            {
                using (var connection = _appDbContext.Connection)
                {
                    var ColumAssign = $"company_id as CompanyId, company_name as CompanyName, " + 
                                      $"company_url as CompanyUrl, company_gst_no as GSTNumber, " +
                                      $"company_tin_no as TinNumber, license_no as LicenseNumber, " +
                                      $"address_id as AddressId, contact_id as ContactId, " +
                                      $"tenant_id as TenantId, source_name as SourceName, " +
                                      //$"convinience_fees_type as FeesType, convinience_fees_amt as FeesAmount, " +
                                      $"created_by as CreatedBy, modified_by as ModifiedBy";

                    var selQuery = $"select " + ColumAssign + " from Logisticmate.company_obj ";

                    var whereCondi = "";

                    if(!string.IsNullOrEmpty(companyId))
                        whereCondi += "company_id = '" + companyId + "'";

                    if(!string.IsNullOrEmpty(tenantId) && String.IsNullOrEmpty(whereCondi))
                        whereCondi += "tenant_id = '" + tenantId + "'";
                    else if(!string.IsNullOrEmpty(tenantId) && !String.IsNullOrEmpty(whereCondi))
                        whereCondi += " and tenant_id = '" + tenantId + "'";

                    var activeCond = "";
                    if(String.IsNullOrEmpty(whereCondi))
                        activeCond = $"where status = '" + Status.Active.ToString() + "'";
                    else
                        activeCond = $" and status = '" + Status.Active.ToString() + "'";

                    if(!string.IsNullOrEmpty(whereCondi))
                        whereCondi = "where " + whereCondi;

                    var orderCond = $" order by company_name ASC ";

                    var sqlSelQuery = selQuery + whereCondi + activeCond + orderCond;
                    var sql = await connection.QueryAsync<CompanyDetails>(sqlSelQuery);
                    retCompanyDetailsList = sql.ToList();

                    AddressContactDetails addressContactDetails = new AddressContactDetails();
                    foreach (CompanyDetails singleCompanyDetails in retCompanyDetailsList)
                    {
                        singleCompanyDetails.ConvinienceFeesDetails = new ConvinienceFeesDetails();
                        singleCompanyDetails.ConvinienceFeesDetails = await GetConvinienceFeesDetails(singleCompanyDetails.CompanyId);

                        addressContactDetails.contactDetails = new ContactDetailsRequest();
                        addressContactDetails.addressDetails = new AddressDetailsRequest();
                        addressContactDetails.contactDetails.ContactId = singleCompanyDetails.ContactId;
                        addressContactDetails.addressDetails.AddressId = singleCompanyDetails.AddressId;
                        if (await _piiServiceRepository.GetContactAddressId(addressContactDetails))
                        {
                            singleCompanyDetails.ContactDetails = addressContactDetails.contactDetails;
                            singleCompanyDetails.AddressDetails = addressContactDetails.addressDetails;
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retCompanyDetailsList;
        }

        public async Task<bool> CheckCompanyNameAvailability(string tenantId, string companyId, string companyName)
        {
            using (var connection = _appDbContext.Connection)
            {
                var tableName = $"company_obj";
                var whereCond = $" where tenant_id = @TenantId" +
                                $" and status = " + (int)Status.Active;

                if(!string.IsNullOrEmpty(companyName))
                    whereCond += $" and company_name = @CompanyName";

                if(!string.IsNullOrEmpty(companyId))
                    whereCond += $" and company_id != @CompanyId";

                object colValueParam = new
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    CompanyName = companyName
                };
                var sqlSelQuery = $"select * from " + tableName + whereCond;
                var sqlResult = await connection.QueryAsync<string>(sqlSelQuery, colValueParam);
                return sqlResult.ToList().Any();
            }
        }

        public async Task<bool> CreateCompany(CompanyRequest companyRequest)
        {
            try
            {
                var uuid = GenerateUUID();
                bool sqlResult = true;
                companyRequest.CompanyId = uuid;

                var tableName = $"Logisticmate.company_obj";

                var colName = $"company_id, company_name, company_url, " +
                              $"tenant_id, source_name, company_gst_no, " +
                              $"company_tin_no, license_no, " +
                              $"address_id, contact_id, " +
                              $"status, created_by, created_on";

                var colValueName = $"@CompanyId, @CompanyName, @CompanyUrl, " +
                                   $"@TenantId, @SourceName, @GSTNumber, " + 
                                   $"@TinNumber, @LicenseNumber, " +
                                   $"@AddressId, @ContactId, " +
                                   $"@Status, @CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                if(sqlResult)
                    sqlResult = await UpdateCompanyIdInUserTable(companyRequest);

                if(String.IsNullOrEmpty(companyRequest.AddressId) || companyRequest.AddressId == "string")
                {
                    AddressContactDetails addressContactDetails = new AddressContactDetails();
                    if(companyRequest.AddressDetails == null)
                        companyRequest.AddressDetails = new AddressDetailsRequest();

                    if(companyRequest.ContactDetails == null)
                        companyRequest.ContactDetails = new ContactDetailsRequest();

                    addressContactDetails.addressDetails = companyRequest.AddressDetails;
                    addressContactDetails.contactDetails = companyRequest.ContactDetails;
                    sqlResult = await _piiServiceRepository.CreateContactAddressId(addressContactDetails);
                    companyRequest.AddressId = companyRequest.AddressDetails.AddressId;
                    companyRequest.ContactId = companyRequest.ContactDetails.ContactId;
                }

                object colValueParam = new
                {
                    CompanyId = companyRequest.CompanyId,
                    CompanyName = companyRequest.CompanyName,
                    CompanyUrl = companyRequest.CompanyUrl,
                    TenantId = companyRequest.TenantId,
                    SourceName = companyRequest.SourceName,
                    GSTNumber = companyRequest.GSTNumber,
                    TinNumber = companyRequest.TinNumber,
                    LicenseNumber = companyRequest.LicenseNumber,
                    AddressId = companyRequest.AddressId,
                    ContactId =  companyRequest.ContactId,
                    Status = Status.Active.ToString(),
                    CreatedBy = companyRequest.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };

                if(sqlResult)
                {
                    using (var connection = _appDbContext.Connection)
                    {
                        sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlInsQuery, colValueParam));
                    }

                    if(sqlResult)
                    {
                        CompanyBranchRequest companyBranchRequest = new CompanyBranchRequest();
                        companyBranchRequest.BranchName = companyRequest.CompanyName;
                        companyBranchRequest.BranchUrl = companyRequest.CompanyUrl;
                        companyBranchRequest.CompanyId = companyRequest.CompanyId;
                        companyBranchRequest.DefaultBranch = "yes";
                        companyBranchRequest.LicenseNumber = companyRequest.LicenseNumber;
                        companyBranchRequest.AddressId = companyRequest.AddressId;
                        companyBranchRequest.ContactId = companyRequest.ContactId;
                        companyBranchRequest.IsUpdate = false;
                        companyBranchRequest.CreatedBy = companyRequest.CreatedBy;
                        sqlResult = await _companyBranchRepository.CreateCompanyBranch(companyBranchRequest);
                    }
                }
                return sqlResult;
            }
            catch (Exception Err)
            {
                companyRequest.ErrorMsg = Err.Message.ToString();
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

        public async Task<bool> UpdateCompany(CompanyRequest companyRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"Logisticmate.company_obj";
                var colName = $"company_id = @CompanyId, company_name = @CompanyName, " +
                              $"company_url = @CompanyUrl, company_gst_no = @GSTNumber, " +
                              $"tenant_id = @TenantId, source_name = @SourceName, " + 
                              $"company_tin_no = @TinNumber, license_no = @LicenseNumber, " +
                              $"address_id = @AddressId, contact_id = @ContactId, " +
                              $"status = @Status, modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where company_id = @CompanyId";

                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                if(sqlResult)
                    sqlResult = await UpdateCompanyIdInUserTable(companyRequest);

                if(String.IsNullOrEmpty(companyRequest.AddressId) || companyRequest.AddressId == "string")
                {
                    AddressContactDetails addressContactDetails = new AddressContactDetails();
                    if(companyRequest.AddressDetails == null)
                        companyRequest.AddressDetails = new AddressDetailsRequest();

                    if(companyRequest.ContactDetails == null)
                        companyRequest.ContactDetails = new ContactDetailsRequest();

                    addressContactDetails.addressDetails = companyRequest.AddressDetails;
                    addressContactDetails.contactDetails = companyRequest.ContactDetails;
                    sqlResult = await _piiServiceRepository.CreateContactAddressId(addressContactDetails);
                    companyRequest.AddressId = companyRequest.AddressDetails.AddressId;
                    companyRequest.ContactId = companyRequest.ContactDetails.ContactId;
                }
                else if(companyRequest.ContactDetails != null && companyRequest.AddressDetails != null)
                {
                    AddressContactDetails addressContactDetails = new AddressContactDetails();
                    addressContactDetails.addressDetails = companyRequest.AddressDetails;
                    addressContactDetails.contactDetails = companyRequest.ContactDetails;
                    sqlResult = await _piiServiceRepository.UpdateContactAddressId(addressContactDetails);
                }

                if(sqlResult)
                {
                    CompanyBranchRequest companyBranchRequest = new CompanyBranchRequest();
                    companyBranchRequest.BranchName = companyRequest.CompanyName;
                    companyBranchRequest.BranchUrl = companyRequest.CompanyUrl;
                    companyBranchRequest.CompanyId = companyRequest.CompanyId;
                    companyBranchRequest.DefaultBranch = "yes";
                    companyBranchRequest.LicenseNumber = companyRequest.LicenseNumber;
                    companyBranchRequest.AddressId = companyRequest.AddressId;
                    companyBranchRequest.ContactId = companyRequest.ContactId;
                    companyBranchRequest.IsUpdate = true;
                    companyBranchRequest.CreatedBy = companyRequest.CreatedBy;
                    sqlResult = await _companyBranchRepository.UpdateCompanyBranch(companyBranchRequest);
                }

                object colValueParam = new
                {
                    TenantId = companyRequest.TenantId,
                    SourceName = companyRequest.SourceName,
                    CompanyId = companyRequest.CompanyId,
                    CompanyName = companyRequest.CompanyName,
                    CompanyUrl = companyRequest.CompanyUrl,
                    GSTNumber = companyRequest.GSTNumber,
                    TinNumber = companyRequest.TinNumber,
                    LicenseNumber = companyRequest.LicenseNumber,
                    AddressId = companyRequest.AddressId,
                    ContactId =  companyRequest.ContactId,
                    Status = Status.Active.ToString(),
                    ModifiedBy = companyRequest.ModifiedBy,
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
                companyRequest.ErrorMsg = Err.Message.ToString();
                return false;
            }
        }
        async Task<bool> UpdateCompanyIdInUserTable(CompanyRequest companyRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"AP_User.user_obj";
                var colName = $"company_id = @CompanyId, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where (user_type = 4 or user_type = 6)" +
                                $" and tenant_id = @TenantId" +
                                $" and source_name = @SourceName";

                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    TenantId = companyRequest.TenantId,
                    SourceName = companyRequest.SourceName,
                    CompanyId = companyRequest.CompanyId,
                    ModifiedBy = companyRequest.ModifiedBy,
                    ModifiedOn = DateTime.UtcNow
                };

                using (var connection = _appDbContext.Connection)
                {
                    sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlUpdateQuery, colValueParam));
                    return sqlResult;
                }
            }
            catch (Exception Err)
            {
                //var Error = Err.Message.ToString();
                companyRequest.ErrorMsg = Err.Message.ToString();
                return false;
            }
        }

        public async Task<bool> DeleteCompany(DeleteRequest request)
        {
            try
            {
                var tableName = $"Logisticmate.company_obj";
                var colName = $"status = @Status, modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where company_id = @CompanyId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    CompanyId = request.Id,
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

        public async Task<ConvinienceFeesDetails> GetConvinienceFeesDetails(string companyId)
        {
            ConvinienceFeesDetails retConvinienceFeesDetails = new ConvinienceFeesDetails();
            try
            {
                var tableName = $"Logisticmate.company_obj";

                var ColumAssign = $"company_id as CompanyId, company_name as CompanyName, " + 
                                  $"convinience_fees_type as FeesType, convinience_fees_amt as FeesAmount, " +
                                  $"modified_by as ModifiedBy";

                var selQuery = $"select " + ColumAssign + " from " +tableName;

                var whereCondi = "";

                var activeCond = $" and status = '" + Status.Active.ToString() + "'";

                if(!string.IsNullOrEmpty(companyId))
                    whereCondi += " where company_id = '" + companyId + "'";

                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelQuery = selQuery + whereCondi + activeCond;
                    var sql = await connection.QueryAsync<ConvinienceFeesDetails>(sqlSelQuery);
                    retConvinienceFeesDetails = sql.FirstOrDefault();
                }
            }
            catch (Exception Err)
            {
                retConvinienceFeesDetails.ErrorMsg = Err.Message.ToString();
            }
            return retConvinienceFeesDetails;
        }
        public async Task<bool> UpdateConvinienceFees(ConvinienceFeesDetails convinienceFeesDetails)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"Logisticmate.company_obj";
                var colName = $"convinience_fees_type = @FeesType, convinience_fees_amt = @FeesAmount, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where company_id = @CompanyId";

                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    CompanyId = convinienceFeesDetails.CompanyId,
                    FeesType = convinienceFeesDetails.FeesType,
                    FeesAmount = convinienceFeesDetails.FeesAmount,
                    ModifiedBy = convinienceFeesDetails.ModifiedBy,
                    ModifiedOn = DateTime.UtcNow
                };

                using (var connection = _appDbContext.Connection)
                {
                    sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlUpdateQuery, colValueParam));
                }
                return sqlResult;
            }
            catch (Exception Err)
            {
                convinienceFeesDetails.ErrorMsg = Err.Message.ToString();
                return false;
            }
        }

    }
}