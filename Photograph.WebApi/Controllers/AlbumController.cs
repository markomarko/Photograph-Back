using Photograph.BLL.Services;
using Photograph.BLL.Services.AlbumService;
using Photograph.WebApi.Adapters;
using Photograph.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Photograph.WebApi.Controllers
{

    //TO DO:
    //PUT method and implemtation in BLL(Update)
    [Authorize]
    [EnableCors("*", "*", "*")]
    public class AlbumController : ApiController
    {
        private readonly IAlbumService _albumService;

        private readonly IPhotoService _photoService;

        public AlbumController(IAlbumService albumService, IPhotoService photoService)
        {
            _albumService = albumService;

            _photoService = photoService;
        }

        public IHttpActionResult Get (Guid id)
        {
            var requestorId = GetRequestorId();
            var albums =
                (_albumService.GetAlbumsById(id, requestorId)).Select(AlbumViewModelAdapter.BuildAlbumViewModel);

            return Ok(albums);
        }

        public IHttpActionResult Post(AlbumViewModel albumViewModel)
        {
            var requestorid = GetRequestorId();

            var albumDto = AlbumViewModelAdapter.BuildAlbumDto(albumViewModel);

            _albumService.Create(albumDto, requestorid);

            var response = Request.CreateResponse(HttpStatusCode.Created, albumViewModel);

            return ResponseMessage(response);
        }

        //[HttpGet]
        //[Route("api/Album/GetAlbum")]
        //public IHttpActionResult GetAlbum(Guid id)
        //{
        //    var requestorId = GetRequestorId();
        //    var albums =
        //        (_albumService.GetAlbumsById(id, requestorId)).Select(AlbumViewModelAdapter.BuildAlbumViewModel);

        //    return Ok(albums);
        //}

        public IHttpActionResult Delete(Guid id)
        {
            var requestorId = GetRequestorId();

            _albumService.Delete(id, requestorId);

            var response = Request.CreateResponse(HttpStatusCode.NoContent);

            return ResponseMessage(response);
        }

        private Guid GetRequestorId()
        {
            var caller = User as ClaimsPrincipal;
            var requestorId = Guid.Parse(caller.FindFirst("id").Value);
            return requestorId;
        }

    }
}
