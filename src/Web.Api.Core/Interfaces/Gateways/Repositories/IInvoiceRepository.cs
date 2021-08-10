using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IInvoiceRepository 
    {
        Task<int> GenerateInvoiceNo(string companyId, string invoiceType);
        Task<bool> CreateInvoice(InvoiceRequest invoiceRequest);
        Task<bool> CheckInvoiceNoAvailability(string companyId, string invoiceType, string invoiceNo);
        Task<List<InvoiceDetails>> GetInvoiceDetails(string companyId, string invoiceId, string invoiceType, string invoiceStatus, string filterStatus);
    }
}