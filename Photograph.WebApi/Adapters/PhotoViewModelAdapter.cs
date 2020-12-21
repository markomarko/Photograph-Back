using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressMapper.Extensions;
using Photograph.BLL.Dtos;
using Photograph.WebApi.Models;

namespace Photograph.WebApi.Adapters
{
    public class PhotoViewModelAdapter
    {
        public static PhotoViewModel BuildPhotoViewModel(PhotoDto obj)
        {
            return obj.Map<PhotoDto, PhotoViewModel>();
        }

        public static PhotoDto BuildPhotoDto(PhotoViewModel obj)
        {
            return obj.Map<PhotoViewModel, PhotoDto>();
        }
    }
}