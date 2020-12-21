using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Photograph.BLL.Adapters;
using Photograph.BLL.Dtos;
using Photograph.BLL.Services.UserManagement;
using Photograph.DAL.Repository.Ports;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository;
using Photograph.BLL.Services.AlbumService;
using Photograph.DAL.Repository.Adapters;

namespace Photograph.BLL.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IGenericRepository<Photo, Guid> _gPhotoRepository;
	    private readonly IPhotoRepository _photoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAlbumService _albumRepository;
      

        public PhotoService(IGenericRepository<Photo, Guid> gPhotoRepository, IUserRepository userRepository, IAlbumService albumRepository, IPhotoRepository photoRepository)
        {
            _gPhotoRepository = gPhotoRepository;
            _userRepository = userRepository;
            _albumRepository = albumRepository;
	        _photoRepository = photoRepository;
        }

        public IEnumerable<PhotoDto> GetPhotosById(Guid id, Guid requestorid, PagingParameterDto paging)
        {
            if ((_userRepository.GetUserById(requestorid)).Roles[0].Name.Equals("Subscriber") || (_userRepository.GetUserById(requestorid)).Roles[0].Name.Equals("Admin"))
            {
                int start = (paging.pageNumber - 1) * paging._pageSize;

                var allPhotos = _gPhotoRepository.GetRange(start, paging._pageSize, photo => (photo.AlbumId == id), photo => photo.Id);
                var photosById = allPhotos as IList<Photo> ?? allPhotos.ToList();
                var photosDto = photosById.Select(x => PhotoAdapter.BuildPhotoDto(x));

                return photosDto;
            }
            else if ((_userRepository.GetUserById(requestorid)).Roles[0].Name.Equals("User"))
            {
                var albums = _albumRepository.GetAlbumsById(requestorid, requestorid);
                var album = albums.SingleOrDefault(x => x.Id.Equals(id));

                if (!album.Equals(null))
                {
                    int start = (paging.pageNumber - 1) * paging._pageSize;

                    var allPhotos = _gPhotoRepository.GetRange(start, paging._pageSize, photo => (photo.AlbumId.Equals(id)), photo => photo.Id);
                    var user = _userRepository.GetUserById(requestorid);
                    foreach (var photo in allPhotos)
                    {
                        if (photo.SelectedByUsers != null)
                        {
                            if (photo.SelectedByUsers.Any(x => x.Id.Equals(user.Id)))
                                photo.Selected = true;
                        }
                        else
                            photo.Selected = false;      
                    }

                    var photosDto = allPhotos.Select(PhotoAdapter.BuildPhotoDto);

                    return photosDto;
                }
            }
            return null;
        }

        public void Create(List<PhotoDto> photoDto, Guid requesotrId)
        {
            if ((_userRepository.GetUserById(requesotrId)).Roles[0].Name.Equals("Subscriber") || (_userRepository.GetUserById(requesotrId)).Roles[0].Name.Equals("Admin"))
            {
               foreach(var photo in photoDto) {
                    var temp = PhotoAdapter.BuildPhoto(photo);
                    _gPhotoRepository.Add(temp);
                };
            }
        }

        public void Update(Guid id, bool selected, Guid requestorid, Guid userid)
        {
            if ((_userRepository.GetUserById(requestorid)).Roles[0].Name.Equals("User"))
                if ((_userRepository.GetUserById(requestorid)).DependsOnAdmin.Any(sub => sub.UserId.Equals(userid)))
                {
                    var dbPhoto = _gPhotoRepository.Get(id);

                    if (dbPhoto != null)
                    {
                        var user = _userRepository.GetUserById(requestorid);

                        if (selected.Equals(true))
                        {
                            if (dbPhoto.SelectedByUsers == null)
                                dbPhoto.SelectedByUsers = new List<User>();
                            dbPhoto.SelectedByUsers.Add(user);
                        }
                        else
                            dbPhoto.SelectedByUsers.Remove(user);

                        _gPhotoRepository.Update(dbPhoto);
                    }
                }
        }

        public void Delete(Guid id, Guid requestorId)
        {
            if ((_userRepository.GetUserById(requestorId)).Roles[0].Name.Equals("Subscriber") || (_userRepository.GetUserById(requestorId)).Roles[0].Name.Equals("Admin"))
                {
                    var dbPhoto = _gPhotoRepository.Get(id);

                    if ((dbPhoto != null) && (dbPhoto.UserId.Equals(requestorId))) _gPhotoRepository.Remove(dbPhoto);
                }
        }

        public int Count(Guid id, Guid requestorId)
        {
            if ((_userRepository.GetUserById(requestorId)).Roles[0].Name.Equals("Subscriber") || (_userRepository.GetUserById(requestorId)).Roles[0].Name.Equals("Admin") || (_userRepository.GetUserById(requestorId)).Roles[0].Name.Equals("User"))
            {
                var count = _photoRepository.Count(id);

                return count;
            }

            return 0;
        }
    }
}