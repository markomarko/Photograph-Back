using System.Configuration;
using IdentityServer3.AccessTokenValidation;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Photograph.WebApi.Adapters;

[assembly: OwinStartup(typeof(Photograph.WebApi.Startup))]

namespace Photograph.WebApi
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
			{
				Authority = ConfigurationManager.AppSettings["Authority"],
				ValidationMode = ValidationMode.ValidationEndpoint,
				RequiredScopes = new[] {"Photograph"}
			});

			MapperScheme.RegisterMapping();

			app.Map("/signalr", map =>
			{
				map.UseCors(CorsOptions.AllowAll);

				var hubConfiguration = new HubConfiguration();
				map.RunSignalR(hubConfiguration);
			});
		}
	}
}