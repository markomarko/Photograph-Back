using Photograph.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photograph.DAL.Repository.Ports
{
    public interface IAlbumRepository
    {
        void AddAlbum(Album entites, List<Guid> clientList);
    }
}
