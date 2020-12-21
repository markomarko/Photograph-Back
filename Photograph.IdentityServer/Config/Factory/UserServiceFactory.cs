using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using Photograph.BLL.Services.IdentityServerServices;

namespace Photograph.IdentityServer.Config.Factory
{
	public class UserServiceFactory : IdentityServer3.Core.Services.IUserService
	{
		private readonly IUserService _userService;

		public UserServiceFactory(IUserService userService)
		{
			_userService = userService;
		}

		public Task PreAuthenticateAsync(PreAuthenticationContext context)
		{
			return Task.FromResult(0);
		}

		public async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
		{
			var user = await _userService.GetUser(context.UserName, context.Password.Sha256());

			if (user != null)
			{
				context.AuthenticateResult = new AuthenticateResult(user.Subject, user.Username);
			}
		}

		public Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
		{
			return Task.FromResult(0);
		}

		public Task PostAuthenticateAsync(PostAuthenticationContext context)
		{
			return Task.FromResult(0);
		}

		public Task SignOutAsync(SignOutContext context)
		{
			return Task.FromResult(0);
		}

		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			var user = await _userService.GetUser(context.Subject.GetSubjectId());

			var list = new List<Claim>()
			{
				new Claim("sub", user.Subject)
			};

			list.AddRange(user.Claims);

			if (!context.AllClaimsRequested)
				list = list.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
			context.IssuedClaims = list;
		}

		public Task IsActiveAsync(IsActiveContext context)
		{
			return Task.FromResult(0);
		}

		private static double ToUnixTimeSeconds(DateTime dateTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var unixDateTime = (dateTime.ToUniversalTime() - epoch).TotalSeconds;

			return unixDateTime;
		}
	}
}