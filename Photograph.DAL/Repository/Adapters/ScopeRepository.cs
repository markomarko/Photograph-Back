using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository.Ports;

namespace Photograph.DAL.Repository.Adapters
{
    public class ScopeRepository:IScopeRepository
    {
        public async Task<Scope> GetScopeById(int id)
        {
            Scope scope;
            using (var context = new PhotographContext())
            {
                scope = await context.Scopes.FirstOrDefaultAsync(x => x.Id.Equals(id));
            }
            return scope;
        }

        public async Task<IEnumerable<Scope>> GetAllScopes()
        {
            IEnumerable<Scope> scopes;
            using (var context = new PhotographContext())
            {
                scopes = await context.Scopes.ToListAsync();
            }
            return scopes;

        }

        public async Task AddScope(Scope scope)
        {
            using (var context = new PhotographContext())
            {
                context.Scopes.Add(scope);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateScope(Scope scope)
        {
            using (var context = new PhotographContext())
            {
                var dbScope = await context.Scopes.FirstOrDefaultAsync(x => x.Id.Equals(scope.Id));
                if (dbScope != null)
                {
                    dbScope.IncludeAllClaimsForUser = scope.IncludeAllClaimsForUser;
                    dbScope.Name = scope.Name;
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteScope(int id)
        {
            using (var context = new PhotographContext())
            {
                var scope = await context.Scopes.FirstOrDefaultAsync(x => x.Id.Equals(id));
                context.Scopes.Remove(scope);
                await context.SaveChangesAsync();
            }
        }
    }
}
