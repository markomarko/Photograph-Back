using System;
using System.Security.Claims;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Ninject.Web.WebApi;
using Ninject;
using Photograph.BLL.Services.UserManagement;
using Photograph.WebApi.Exceptions;

namespace Photograph.WebApi.Interceptors
{
	public class SuspendedUsersFilter : ActionFilterAttribute
	{
		[Inject]
		public IUserManagementService _userManagementService { get; set; }

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
			if (identity != null && identity.FindFirst("sub") != null)
			{
				var id = Guid.Parse(identity.FindFirst("sub").Value);
				var user = _userManagementService.GetUser(id, id);
				if (user.IsSuspended)
					throw new SuspendedUserException();
			}

			base.OnActionExecuting(actionContext);
		}
	}
}