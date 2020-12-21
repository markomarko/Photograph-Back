using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer3.Core.Models;
using System.Text;
using System.Threading.Tasks;
using Photograph.DAL.Repository.Ports;

namespace Photograph.BLL.Services.IdentityServerServices
{
    public class ScopeService:IScopeService
    {
        private readonly IScopeRepository _scopeRepository;

        public ScopeService(IScopeRepository scopeRepository)
        {
            _scopeRepository = scopeRepository;
        }
        public async Task<IEnumerable<Scope>> GetScopes()
        {
            return (await _scopeRepository.GetAllScopes()).Select(scope => new Scope()
            {
                Name = scope.Name,
                IncludeAllClaimsForUser = scope.IncludeAllClaimsForUser,
                Enabled = true,
                Type = ScopeType.Resource
            }).ToList();
        }
    }
}
