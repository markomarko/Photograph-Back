using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.BLL.Dtos;
using Photograph.DAL.Entities;
using ExpressMapper.Extensions;

namespace Photograph.BLL.Adapters
{
	public class PhotoAdapter
	{
        private const string base64Header = "data:image/jpeg;base64,";

        public static PhotoDto BuildPhotoDto(Photo obj)
		{
			var photoDto =  obj.Map<Photo, PhotoDto>();

            var byteContext = Encoding.Default.GetBytes(photoDto.Context);

            photoDto.Context = base64Header + Convert.ToBase64String(byteContext, Base64FormattingOptions.None);

            return photoDto;
        }

		public static Photo BuildPhoto(PhotoDto obj)
		{
			var photo =  obj.Map<PhotoDto, Photo>();

            photo.Context = photo.Context.Substring(base64Header.Length);

            var byteContext = Convert.FromBase64String(photo.Context);

            photo.Context = Encoding.Default.GetString(byteContext, 0, byteContext.Length);

            return photo;
        }
	}
}