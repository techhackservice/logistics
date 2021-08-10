using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.UseCases;


namespace Web.Api.Core.UseCases
{
    public sealed class CustomerUseCases : ICustomerUseCases
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerUseCases(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;
            getDetailsResponse = new GetDetailsResponse(await _customerRepository.GetCustomerDetails(request.Id), true, "Data Fetched Successfully");
            outputPort.Handle(getDetailsResponse);
            return true;
        }
    }
}