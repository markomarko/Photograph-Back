using ExpressMapper.Extensions;
using Photograph.BLL.Dtos;
using Photograph.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photograph.BLL.Adapters
{
    public class AlbumAdapter
    {
        public static AlbumDto BuildAlbumDto(Album obj)
        {
            return obj.Map<Album, AlbumDto>();
        }

        public static Album BuildAlbum(AlbumDto obj)
        {
            return obj.Map<AlbumDto, Album>();
        }
    }
}
