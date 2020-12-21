using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Photograph.BLL.Services.IdentityServerServices;

namespace Photograph.IdentityServer.Config.Factory
{
    public class ScopeStoreFactory : IScopeStore
    {
        private readonly IScopeService _scopeService;

        public ScopeStoreFactory(IScopeService scopeService)
        {
            _scopeService = scopeService;
        }

        public async Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            return (await _scopeService.GetScopes()).Where(s => scopeNames.Contains(s.Name));
        }

        public async Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            var scopes = await _scopeService.GetScopes();

            return publicOnly ? scopes.Where(s => s.ShowInDiscoveryDocument) : scopes;
        }
    }
}