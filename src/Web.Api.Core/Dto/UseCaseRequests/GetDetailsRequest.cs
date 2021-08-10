using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class GetDetailsRequest  : IUseCaseRequest<GetDetailsResponse>
    {
        public string Id { get; set; }
        public string TrackId { get; set; }
        public string Email { get; set; }
        public string CompanyUrl { get; set; }
        public string CompanyId { get; set; }
        public string TenantId { get; set; }
        public string OrderId { get; set; }
        public string InvoiceId { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceStatus { get; set; }
        public string InvoiceFilterStatus { get; set; }
        public bool IsConvinienceFees { get; set; }
        public bool IsGetInvoiceNo { get; set; }

        public GetDetailsRequest(string id)
        {
            Id = id;
        }
        public GetDetailsRequest(string id, string companyId, bool isDefault)
        {
            Id = id;
            CompanyId = companyId;
        }
        public GetDetailsRequest(string companyId, bool isGetInvoiceNo, string invoiceType)
        {
            CompanyId = companyId;
            IsGetInvoiceNo = isGetInvoiceNo;
            InvoiceType = invoiceType;
        }
        public GetDetailsRequest(string companyId, string tenantId)
        {
            CompanyId = companyId;
            TenantId = tenantId;
        }
        public GetDetailsRequest(string companyId, bool isConvinienceFees)
        {
            CompanyId = companyId;
            IsConvinienceFees = isConvinienceFees;
        }
        public GetDetailsRequest(string trackId, string email, string companyUrl)
        {
            TrackId = trackId;
            Email = email;
            CompanyUrl = companyUrl;
        }
        public GetDetailsRequest(string companyId, string invoiceId, string invoiceType, string invoiceStatus, string invoiceFilterStatus)
        {
            CompanyId = companyId;
            InvoiceId = invoiceId;
            InvoiceType = invoiceType;
            InvoiceStatus = invoiceStatus;
            InvoiceFilterStatus = invoiceFilterStatus;
        }
    }
}