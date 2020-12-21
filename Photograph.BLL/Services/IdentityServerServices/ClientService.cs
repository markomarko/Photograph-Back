using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using Photograph.DAL.Repository.Ports;

namespace Photograph.BLL.Services.IdentityServerServices
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client> GetClientByClientId(string clientId)
        {
            var client =
                (await _clientRepository.GetAllClients()).SingleOrDefault(
                    x => x.ClientId.Equals(clientId) && x.Enabled.Equals(true));
            if (client == null) return null;

            var scopeList = new List<string>();

            client.AllowedScopes.FindAll(x => true).ForEach(x => scopeList.Add(x.Name));
            var tempClient = new Client()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                Enabled = client.Enabled,
                ClientSecrets = new List<Secret>() {new Secret(client.ClientSecrets, "")},
                Flow = (Flows) client.Flow,
                AccessTokenLifetime = client.AccessTokenLifetime,
                AllowedScopes = new List<string>(scopeList)
            };

            return tempClient;
        }
    }
}