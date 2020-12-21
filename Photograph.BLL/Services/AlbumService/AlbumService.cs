using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.BLL.Dtos;
using Photograph.DAL.Repository;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository.Ports;
using Photograph.BLL.Adapters;

namespace Photograph.BLL.Services.AlbumService
{
    public class AlbumService : IAlbumService
    {
        private readonly IGenericRepository<Album, Guid> _gAlbumRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Photo, Guid> _photoRepository;

        public AlbumService(IGenericRepository<Album, Guid> gAlbumRepository, IAlbumRepository albumRepository, IUserRepository userRepository, IGenericRepository<Photo, Guid> photoRepository)
        {
            _gAlbumRepository = gAlbumRepository;
            _albumRepository = albumRepository;
            _userRepository = userRepository;
            _photoRepository = photoRepository;
        }

        public AlbumDto GetAlbum(Guid albumId)
        {
            return AlbumAdapter.BuildAlbumDto(_gAlbumRepository.Get(albumId));
        }

        public void Create(AlbumDto albumDto, Guid requestorId)
        {
            if ((_userRepository.GetUserById(requestorId)).Roles[0].Name.Equals("Subscriber") || ( _userRepository.GetUserById(requestorId)).Roles[0].Name.Equals("Admin"))
            {
                _albumRepository.AddAlbum(AlbumAdapter.BuildAlbum(albumDto), albumDto.UsersWithAccess);
            }

        }

        public void Delete(Guid id, Guid requestorId)
        {
            if ((_userRepository.GetUserById(requestorId)).Roles[0].Name.Equals("Subscriber") || (_userRepository.GetUserById(requestorId)).Roles[0].Name.Equals("Admin"))
            {
                var dbAlbum = _gAlbumRepository.Get(id);

                if (dbAlbum.OwnerId.Equals(requestorId))
                {
                    var dbPhotos = _photoRepository.Find(x => x.AlbumId.Equals(id)).ToList();

                    foreach (var photo in dbPhotos)
                    {
                        _photoRepository.Remove(photo);
                    }
                }

                _gAlbumRepository.Remove(dbAlbum);
                    
            }

        }

        public IEnumerable<AlbumDto> GetAlbumsById(Guid id, Guid requestorid)
        {
            if (id.Equals(requestorid))
            {
                if ((_userRepository.GetUserById(id)).Roles[0].Name.Equals("Subscriber") || (_userRepository.GetUserById(id)).Roles[0].Name.Equals("Admin"))
                {
                    var allAlbums = _gAlbumRepository.Find(album => album.OwnerId.Equals(id));
                    var albumsbyId = allAlbums as IList<Album> ?? allAlbums.ToList();
                    return allAlbums.Select(AlbumAdapter.BuildAlbumDto).ToList();
                }
                else if ((_userRepository.GetUserById(id)).Roles[0].Name.Equals("User"))
                {
                    var allAlbums = _gAlbumRepository.Find(album => album.UsersWithAccess.Any(user => user.Id.Equals(id)));
                    var albumById = allAlbums as IList<Album> ?? allAlbums.ToList();
                    return albumById.Select(AlbumAdapter.BuildAlbumDto).ToList();
                }
                return null;
            }
            return null;
        }
    }
}
