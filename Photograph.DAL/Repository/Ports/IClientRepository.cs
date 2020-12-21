using System.Collections.Generic;
using System.Threading.Tasks;
using Photograph.DAL.Entities;

namespace Photograph.DAL.Repository.Ports
{
    public interface IClientRepository
    {
        Task<Client> GetClientById(int id);

        Task<IEnumerable<Client>> GetAllClients();

        Task AddClient(Client client);

        Task UpdateClient(Client client);

        Task DeleteClient(int id);
    }
}
