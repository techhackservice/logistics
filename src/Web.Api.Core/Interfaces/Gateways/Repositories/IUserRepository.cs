using System.Threading.Tasks;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IUserRepository 
    {
        Task<bool> CheckUsername(string userName);
    }
}
