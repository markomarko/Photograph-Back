using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Services;
using Ninject.Modules;
using Photograph.IdentityServer.Config.Factory;

namespace Photograph.IdentityServer.App_Start
{
    public class IdentityServerNinjectModule:NinjectModule
    {
        public override void Load()
        {
            Bind<IUserService>().To<UserServiceFactory>();
            Bind<IScopeStore>().To<ScopeStoreFactory>();
            Bind<IClientStore>().To<ClientStoreFactory>();
        }
    }
}