using System.Threading.Tasks;

namespace Web.Api.Core.Interfaces.Services
{
    public interface IHttpClientService
    {
        Task<string> Post(string url, string bodyData);
        Task<string> Put(string url, string bodyData);
        Task<string> Delete(string url, string bodyData);
        Task<string> Get(string url);
    }
}