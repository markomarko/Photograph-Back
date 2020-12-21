using System;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Photograph.BLL.Services.UserManagement;
using Photograph.WebApi.Cache;

namespace Photograph.WebApi.Signalr.Hubs
{
	[HubName("notificationHub")]
	[EnableCors("*", "*", "*", "*")]
	//[Authorize(Roles = "Subscriber, Admin")]
	public class NotificationHub : Hub
	{
		private readonly ISignalRCache _signalRCache;

		public NotificationHub(ISignalRCache signalRCache)
		{
			_signalRCache = signalRCache;
		}

		public override Task OnConnected()
		{
			var id = GetRequesterId();
			var connectionId = Context.ConnectionId;

			_signalRCache.AddConnectionId(id, connectionId);

			Clients.Caller.albumRequest("Message importante");

			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			var id = GetRequesterId();
			_signalRCache.RemoveConnectionId(id);
			return base.OnDisconnected(stopCalled);
		}

		private Guid GetRequesterId()
		{
			var caller = Context.User as ClaimsPrincipal;
			var requesterId = Guid.Parse(caller.FindFirst("sub").Value);
			return requesterId;
		}
	}
}