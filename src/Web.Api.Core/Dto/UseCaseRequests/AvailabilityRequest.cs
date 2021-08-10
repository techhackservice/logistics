using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class AvailabilityRequest : IUseCaseRequest<AvailabilityResponse>
    {
        public string Id { get; }
        public string Name { get; }
        public string CompanyId { get; }
        public string InvoiceType { get; }
        public string InvoiceNo { get; }
        public bool IsGetInvoiceNo { get; }

        public AvailabilityRequest(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public AvailabilityRequest(string companyId, string id, string name)
        {
            Id = id;
            CompanyId = companyId;
            Name = name;
        }
        public AvailabilityRequest(string companyId, string invoiceNo, string invoiceType, bool isGetInvoiceNo)
        {
            CompanyId = companyId;
            InvoiceNo = invoiceNo;
            InvoiceType = invoiceType;
            IsGetInvoiceNo = isGetInvoiceNo;
        }
    }
}