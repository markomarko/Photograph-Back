using System;
using System.Data.Entity;
using Ninject.Modules;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository;
using Photograph.DAL.Repository.Adapters;
using Photograph.DAL.Repository.Ports;

namespace Photograph.DAL.Shared
{
	public class DALNinjectModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IGenericRepository<Photo, Guid>>().To<PhotoRepository>();
			Bind<IGenericRepository<Album, Guid>>().To<AlbumRepository>();
			Bind<IGenericRepository<User, Guid>>().To<UserRepository>();
			Bind<IGenericRepository<Subscriber, Guid>>().To<SubscriberRepository>();
			Bind<IGenericRepository<Role, Guid>>().To<RoleRepository>();
			Bind<IGenericRepository<Notification, Guid>>().To<NotificationRepository>();

			Bind<DbContext>().To<PhotographContext>().InTransientScope();

			Bind<IUserRepository>().To<UserRepository>();
			Bind<IScopeRepository>().To<ScopeRepository>();
			Bind<IClientRepository>().To<ClientRepository>();
            Bind<IPhotoRepository>().To<PhotoRepository>();
            Bind<IAlbumRepository>().To<AlbumRepository>();
		}
	}
}