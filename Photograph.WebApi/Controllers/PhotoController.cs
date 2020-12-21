using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Photograph.BLL.Dtos;
using Photograph.BLL.Services;
using Photograph.BLL.Services.UserManagement;
using Photograph.WebApi.Adapters;
using Photograph.WebApi.Models;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using System.Web;
using Photograph.WebApi.Tools;

namespace Photograph.WebApi.Controllers
{
	[Authorize]
	[EnableCors("*", "*", "*")]

	public class PhotoController : ApiController
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        public IHttpActionResult Get(Guid id, [FromUri] PagingParameterModel paging)
        {
            var requestorId = GetRequestorId();
            var photos =
                (_photoService.GetPhotosById(id, requestorId, PagingParameterViewModelAdapter.BuildPagingDto(paging))).Select(PhotoViewModelAdapter.BuildPhotoViewModel);

            int count = _photoService.Count(id, requestorId);
            var paginationMetadata = PaginationTool.SetPagination(paging, count);


            HttpContext.Current.Response.AddHeader("Access-Control-Expose-Headers", "Paging-Headers");
            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(photos);
        }

        public IHttpActionResult Post(IEnumerable<PhotoViewModel> photoViewModel)
        {
            var requestorid = GetRequestorId();

            List<PhotoDto> photoDto = new List<PhotoDto>();

            photoViewModel.ToList().ForEach(x => photoDto.Add(PhotoViewModelAdapter.BuildPhotoDto(x)));

            _photoService.Create(photoDto, requestorid);

            var response = Request.CreateResponse(HttpStatusCode.Created, photoViewModel);

            return ResponseMessage(response);
        }

        public IHttpActionResult Delete(Guid id)
        {
            var requestorId = GetRequestorId();

            _photoService.Delete(id, requestorId);

            var response = Request.CreateResponse(HttpStatusCode.NoContent);

            return ResponseMessage(response);
        }

        public IHttpActionResult Put(PhotoViewModel obj)
        {
            var requestorId = GetRequestorId();

            var photoDto = PhotoViewModelAdapter.BuildPhotoDto(obj);

            _photoService.Update(obj.Id, obj.Selected, requestorId, obj.UserId);

            return Ok();
        }

        private Guid GetRequestorId()
        {
            var caller = User as ClaimsPrincipal;
            var requestorId = Guid.Parse((caller.FindFirst("id").Value));
            return requestorId;
        }
    }
}