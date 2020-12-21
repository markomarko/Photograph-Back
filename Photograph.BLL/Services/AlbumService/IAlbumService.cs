using Photograph.BLL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photograph.BLL.Services.AlbumService
{
    public interface IAlbumService
    {
        AlbumDto GetAlbum(Guid albumId);
        IEnumerable<AlbumDto> GetAlbumsById(Guid id, Guid requestorid);
		void Create(AlbumDto albumDto, Guid requestorId);
		void Delete(Guid id, Guid requestorId);
    }
}
