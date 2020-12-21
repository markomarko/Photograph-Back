using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;

namespace Photograph.BLL.Services.IdentityServerServices
{
    public interface IScopeService
    {
        Task<IEnumerable<Scope>> GetScopes();
    }
}