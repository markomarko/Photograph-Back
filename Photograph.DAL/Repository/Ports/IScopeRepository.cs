using System.Collections.Generic;
using System.Threading.Tasks;
using Photograph.DAL.Entities;

namespace Photograph.DAL.Repository.Ports
{
    public interface IScopeRepository
    {
        Task<Scope> GetScopeById(int id);

        Task<IEnumerable<Scope>> GetAllScopes();

        Task AddScope(Scope scope);

        Task UpdateScope(Scope scope);

        Task DeleteScope(int id);
    }
}