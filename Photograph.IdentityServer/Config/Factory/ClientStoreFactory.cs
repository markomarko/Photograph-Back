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
    public class ClientStoreFactory : IClientStore
    {
        private readonly IClientService _clientService;

        public ClientStoreFactory(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return await _clientService.GetClientByClientId(clientId);
        }
    }
}