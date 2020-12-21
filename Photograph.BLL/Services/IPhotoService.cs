using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.BLL.Dtos;
using Photograph.DAL.Entities;


namespace Photograph.BLL.Services
{
    public interface IPhotoService
    {
        IEnumerable<PhotoDto> GetPhotosById(Guid id, Guid requestorid, PagingParameterDto paging);
		void Create(List<PhotoDto> photoDto, Guid requestorId);
		void Update(Guid id, bool selected, Guid requestorid, Guid userid);
        void Delete(Guid id, Guid requestorId);
        int Count(Guid id, Guid requestorId);
    }
}