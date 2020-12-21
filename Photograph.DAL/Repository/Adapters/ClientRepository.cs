using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using IdentityServer3.Core.Models;
using System.Threading.Tasks;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository.Ports;
using Client = Photograph.DAL.Entities.Client;

namespace Photograph.DAL.Repository.Adapters
{
    public class ClientRepository : IClientRepository
    {
        public async Task<Client> GetClientById(int id)
        {
            Client client;
            using (var context = new PhotographContext())
            {
                client = await context.Clients.Include("AllowedScopes").FirstOrDefaultAsync(x => x.Id.Equals(id));
            }
            return client;
        }

        public async Task<IEnumerable<Client>> GetAllClients()
        {
            IEnumerable<Client> clients;
            using (var context = new PhotographContext())
            {
                clients = await context.Clients.Include("AllowedScopes").ToListAsync();
            }
            return clients;
        }

        public async Task AddClient(Client client)
        {
            using (var context = new PhotographContext())
            {
                client.ClientSecrets = client.ClientSecrets.Sha256();
                context.Clients.Add(client);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateClient(Client client)
        {
            using (var context = new PhotographContext())
            {
                var dbClient = await context.Clients.FirstOrDefaultAsync(x => x.Id.Equals(client.Id));
                if (dbClient != null)
                {
                    dbClient.Flow = client.Flow;
                    dbClient.Enabled = client.Enabled;
                    dbClient.ClientSecrets = client.ClientSecrets.Sha256();
                    dbClient.ClientName = client.ClientName;
                    dbClient.AccessTokenLifetime = client.AccessTokenLifetime;
                    dbClient.ClientId = client.ClientId;
                    dbClient.AllowedScopes.AddRange(client.AllowedScopes);
                }

                context.SaveChanges();
            }
        }

        public async Task DeleteClient(int id)
        {
            using (var context = new PhotographContext())
            {
                var client = await context.Clients.FirstOrDefaultAsync(x => x.Id.Equals(id));
                context.Clients.Remove(client);
                await context.SaveChangesAsync();
            }
        }
    }
}