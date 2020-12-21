using System.Threading.Tasks;
using IdentityServer3.Core.Models;

namespace Photograph.BLL.Services.IdentityServerServices
{
    public interface IClientService
    {
        Task<Client> GetClientByClientId(string clientId);
    }
}