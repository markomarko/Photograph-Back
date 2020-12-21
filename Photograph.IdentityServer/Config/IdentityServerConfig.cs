using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using Ninject;
using Owin;
using Photograph.IdentityServer.App_Start;

namespace Photograph.IdentityServer.Config
{
    public static class IdentityServerConfig
    {
        private static IKernel _kernel;

        internal static void ConfigureIdentityServer(IAppBuilder app)
        {
            _kernel = NinjectResolver.BuildKernel();

            var userService = _kernel.Get<IUserService>();
            var scopeStore = _kernel.Get<IScopeStore>();
            var clientStore = _kernel.Get<IClientStore>();

            var factory = new IdentityServerServiceFactory
            {
                ScopeStore = new Registration<IScopeStore>(scopeStore),
                ClientStore = new Registration<IClientStore>(clientStore),
                UserService = new Registration<IUserService>(userService),
                CorsPolicyService = new Registration<ICorsPolicyService>(new DefaultCorsPolicyService
                {
                    AllowAll = true,
                })
            };

            factory.Register(new Registration<HttpContext>(resolver => HttpContext.Current));
            factory.Register(
                new Registration<HttpContextBase>(resolver => new HttpContextWrapper(resolver.Resolve<HttpContext>())));
            factory.Register(new Registration<HttpRequestBase>(resolver => resolver.Resolve<HttpContextBase>().Request));
            factory.Register(new Registration<HttpResponseBase>(resolver => resolver.Resolve<HttpContextBase>().Response));
            factory.Register(
                new Registration<HttpServerUtilityBase>(resolver => resolver.Resolve<HttpContextBase>().Server));
            factory.Register(
                new Registration<HttpSessionStateBase>(resolver => resolver.Resolve<HttpContextBase>().Session));

            app.Map("/core", idsrvApp =>
            {
                idsrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    Factory = factory,
                    SigningCertificate = LoadCertificate(),
                    SiteName = "Photograph.IdentityServer",
					RequireSsl = false
                });
            });
        }

        private static X509Certificate2 LoadCertificate()
        {
            //X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            //certStore.Open(OpenFlags.ReadOnly);

            //X509Certificate2Collection certCollection = certStore.Certificates.Find(
            //                           X509FindType.FindByThumbprint,
            //                     // Replace below with your cert's thumbprint
            //                     "6B7ACC520305BFDB4F7252DAEB2177CC091FAAE1",
            //                           false);

            //// Get the first cert with the thumbprint
            ////if (certCollection.Count > 0)
            ////{
            //X509Certificate2 cert = certCollection[0];

            //return cert;
            ////Use certificate
            ////    Console.WriteLine(cert.FriendlyName);
            ////}
            ////certStore.Close();

            var fileName = $@"{AppDomain.CurrentDomain.BaseDirectory}App_Data\idsrv3test.pfx";

            X509Certificate2 cert;
            try
            {
                cert = new X509Certificate2(fileName, "idsrv3test");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return cert;
        }
    }
}