using System.Threading.Tasks;
using IdentityServer3.Core.Services.InMemory;

namespace Photograph.BLL.Services.IdentityServerServices
{
    public interface IUserService
    {
        Task<InMemoryUser> GetUser(string userName, string password);

        Task<InMemoryUser> GetUser(string id);
    }
}