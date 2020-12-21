using Ninject.Modules;
using Photograph.BLL.Services.IdentityServerServices;
using Photograph.WebApi.Adapters;
using Photograph.WebApi.Cache;

namespace Photograph.WebApi
{
    public class WebApiNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISignalRCache>().To<SignalRCache>().InSingletonScope();
        }
    }
}