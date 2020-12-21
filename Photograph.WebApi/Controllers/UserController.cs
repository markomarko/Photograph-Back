using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Photograph.BLL.Dtos;
using Photograph.BLL.Services;
using Photograph.BLL.Services.AlbumService;
using Photograph.BLL.Services.UserManagement;
using Photograph.WebApi.Adapters;
using Photograph.WebApi.Cache;
using Photograph.WebApi.Models;
using Photograph.WebApi.Signalr.Hubs;
using Photograph.WebApi.Tools;

namespace Photograph.WebApi.Controllers
{
	[System.Web.Http.Authorize(Roles = "Admin, Subscriber")]
	[EnableCors("*", "*", "*", "*")]
	public class UserController : ApiController
	{
		private readonly IUserManagementService _userManagementService;
	    private readonly ISignalRCache _signalRCache;
	    private readonly IAlbumService _albumService;
	    private readonly INotificationService _notificationSerice;
			
        public UserController(IUserManagementService userManagementService, ISignalRCache signalRCache, IAlbumService albumService, INotificationService notificationSerice)
        {
            _userManagementService = userManagementService;
            _signalRCache = signalRCache;
            _albumService = albumService;
	        _notificationSerice = notificationSerice;
        }

		public IHttpActionResult Get([FromUri] PagingParameterModel paging)
		{
			var requestorId = GetRequesterId();

			var users = _userManagementService
				.GetAll(requestorId, PagingParameterViewModelAdapter.BuildPagingDto(paging))
				.Select(UserViewModelAdapter.BuiUserViewModel);

			var paginationMetadata = PaginationTool.SetPagination(paging, _userManagementService.CountDependedUsers(requestorId));

			HttpContext.Current.Response.AddHeader("Access-Control-Expose-Headers", "Paging-Headers");
			HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

			return Ok(users);
		}

		public IHttpActionResult Get(string id)
		{
			var requestorId = GetRequesterId();

			var user = _userManagementService.GetUser(Guid.Parse(id), requestorId);

			return Ok(user);
		}

        [System.Web.Http.Authorize(Roles = "Admin, Subscriber")]
        [Route("api/User/GetAll")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var requestorId = GetRequesterId();

            var user = _userManagementService.GetAll(requestorId);

            return Ok(user);
        }

        [System.Web.Http.Authorize(Roles = "Admin")]
		[Route("api/User/{id}/suspend")]
		public IHttpActionResult Get(Guid id)
		{
			_userManagementService.Suspend(id);

			return Ok();
		}

		[System.Web.Http.Authorize(Roles = "Admin")]
		[HttpGet]
		[Route("api/User/{id}/resume")]
		public IHttpActionResult Resume(Guid id)
		{
			_userManagementService.Resume(id);

			return Ok();
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("api/User/client")]
		public IHttpActionResult RegisterUser([FromBody] UserViewModel userViewModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var user = _userManagementService.IsUsernameAvailable(userViewModel.UserName);

			if (!user)
			{
				var response1 = Request.CreateResponse(HttpStatusCode.Forbidden, "Username not available");

				return ResponseMessage(response1);
			}

			var userDto = UserViewModelAdapter.BuildUserDto(userViewModel);

			_userManagementService.Create(userViewModel.SubscriberEmail, userDto);

			var userSub = _userManagementService.GetUserByEmail(userViewModel.SubscriberEmail);

			var id = _signalRCache.GetConnectionId(userSub.Id);

			if (string.IsNullOrEmpty(id)) return BadRequest();

			var notification = new NotificationDto()
			{
				Text = $"User {userDto.UserName} requested access to your albums",
				UserId = userSub.Id
			};
			_notificationSerice.Save(notification);

			var ctx = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
			ctx.Clients.Client(id).albumRequest(notification.Text);

			var response = Request.CreateResponse(HttpStatusCode.Created, userViewModel);

			return ResponseMessage(response);
		}


		[AllowAnonymous]
		[HttpPost]
		[Route("api/User/subscriber")]
		public IHttpActionResult RegisterSubscriber([FromBody] SubscriberViewModel subscriberViewModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var user = _userManagementService.IsUsernameAvailable(subscriberViewModel.UserName);

			if (!user)
			{
				var response1 = Request.CreateResponse(HttpStatusCode.Forbidden, "Username not available");

				return ResponseMessage(response1);
			}

			var subscriberDto = SubscriberViewModelAdapter.BuildSubscriberDto(subscriberViewModel);

			_userManagementService.Create(subscriberViewModel.TokenId, subscriberDto);

			var response = Request.CreateResponse(HttpStatusCode.Created, subscriberViewModel);

			return ResponseMessage(response);
		}

		[HttpPost]
		[Route("api/User/friend")]
		public IHttpActionResult RequestSubscriberFriendship([FromBody] FriendshipRequestViewModel friendshipRequest)
		{
			return Ok();
		}

		[HttpGet]
		[Route("api/User/album")]
        [System.Web.Http.Authorize(Roles = "User")]
		public IHttpActionResult RequestAlbumAccess([FromUri] Guid albumId)
		{
		    var requesterId = GetRequesterId();
		    var user = _userManagementService.GetUser(requesterId, requesterId);

		    var album = _albumService.GetAlbum(albumId);

		    var id = _signalRCache.GetConnectionId(album.OwnerId);

		    if (string.IsNullOrEmpty(id)) return BadRequest();
			var notification = new NotificationDto()
			{
				Text = $"User {user.UserName} requested access to {album.Name}",
				UserId = album.OwnerId
			};
			_notificationSerice.Save(notification);

		    var ctx = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
		    ctx.Clients.Client(id).albumRequest(notification.Text);

			return Ok();
		}

	    [HttpGet]
	    [Route("api/User/signalr/{userKey}")]
	    public IHttpActionResult Signalr(Guid userKey)
	    {
	        var ctx = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
	        var id = _signalRCache.GetConnectionId(userKey);

            ctx.Clients.Client(id).albumRequest($"User message");

	        return Ok();
	    }

		[HttpGet]
		[Route("api/User/signalr/register/{connectionId}")]
		public IHttpActionResult SignalrRegister(string connectionId)
		{
			var ctx = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
			_signalRCache.AddConnectionId(GetRequesterId(), connectionId);

			//ctx.Clients.Client(connectionId).albumRequest(connectionId + GetRequesterId());

			return Ok();
		}

		[HttpGet]
		[Route("api/User/{id}/notifications")]
		public IHttpActionResult GetNotifications(Guid id)
		{

			return Ok(_notificationSerice.GetAll(id));
		}

		public IHttpActionResult Put(UserViewModel userViewModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var userDto = UserViewModelAdapter.BuildUserDto(userViewModel);

			_userManagementService.Edit(userDto, GetRequesterId());

			return Ok();
		}

		public IHttpActionResult Delete(Guid id)
		{
			var requestorId = GetRequesterId();

			_userManagementService.Delete(id, requestorId);

			var response = Request.CreateResponse(HttpStatusCode.NoContent);

			return ResponseMessage(response);
		}



		private Guid GetRequesterId()
		{
			var caller = User as ClaimsPrincipal;
			var requestorId = Guid.Parse(caller.FindFirst("sub").Value);
			return requestorId;
		}
	}
}