using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Photograph.IdentityServer.Config;
using Serilog;

[assembly: OwinStartup(typeof (Photograph.IdentityServer.Startup))]

namespace Photograph.IdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //var fileName = $@"{AppDomain.CurrentDomain.BaseDirectory}Logging\\Log.txt";

            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.RollingFile(fileName)
            //    .CreateLogger();

            IdentityServerConfig.ConfigureIdentityServer(app);
        }
    }
}