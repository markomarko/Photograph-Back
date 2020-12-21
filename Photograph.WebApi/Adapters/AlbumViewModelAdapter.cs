using ExpressMapper.Extensions;
using Photograph.BLL.Dtos;
using Photograph.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photograph.WebApi.Adapters
{
    public class AlbumViewModelAdapter
    {
        public static AlbumViewModel BuildAlbumViewModel(AlbumDto obj)
        {
            return obj.Map<AlbumDto, AlbumViewModel>();
        }

        public static AlbumDto BuildAlbumDto(AlbumViewModel obj)
        {
            return obj.Map<AlbumViewModel, AlbumDto>();
        }
    }
}