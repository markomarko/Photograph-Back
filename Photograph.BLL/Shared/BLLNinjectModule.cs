using Ninject.Modules;
using Photograph.BLL.Services;
using Photograph.BLL.Services.AlbumService;
using Photograph.BLL.Services.IdentityServerServices;
using Photograph.BLL.Services.MailService;
using Photograph.BLL.Services.UserManagement;
using Photograph.DAL.Shared;

namespace Photograph.BLL.Shared
{
	public class BLLNinjectModule : NinjectModule
	{
		public override void Load()
		{
			Kernel.Load(new[] {new DALNinjectModule()});

			Bind<IUserService>().To<UserService>();
			Bind<IScopeService>().To<ScopeService>();
			Bind<IClientService>().To<ClientService>();
			Bind<IPhotoService>().To<PhotoService>();
			Bind<IAlbumService>().To<AlbumService>();
			Bind<INotificationService>().To<NotificationService>();

			Bind<IUserManagementService>().To<UserManagementService>();
			Bind<IEmailService>().To<MailService>();
		}
	}
}