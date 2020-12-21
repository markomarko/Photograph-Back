using System;
using System.Collections.Generic;
using System.Linq;
using ExpressMapper;
using Photograph.BLL.Dtos;
using Photograph.BLL.Services.UserManagement;
using Photograph.DAL.Entities;

namespace Photograph.BLL.Shared
{
	public class MapperScheme
	{
		public static void RegisterMapping()
		{
			Mapper.Register<Notification, NotificationDto>();
			Mapper.Register<NotificationDto, Notification>();

			Mapper.Register<UserDto, User>()
				.Ignore(dest => dest.Roles)
				.Ignore(dest => dest.Subscriber)
				.Ignore(dest => dest.AccessibleAlbums)
				.Ignore(dest => dest.DependsOnAdmin)
				.Ignore(dest => dest.SelectedPhotos);
			Mapper.Register<User, UserDto>().Member(dest => dest.Roles, src => src.Roles == null
				? new List<string>() {RoleConstants.User}
				: src.Roles.Select(x => x.Name).ToList());

			Mapper.Register<AlbumDto, Album>()
                .Ignore(dest => dest.UsersWithAccess);
			Mapper.Register<Album, AlbumDto>();

			Mapper.Register<PhotoDto, Photo>()
                .Ignore(dest => dest.SelectedByUsers)
                .Ignore(dest => dest.Album);
			Mapper.Register<Photo, PhotoDto>();

			Mapper.Register<SubscriberDto, Subscriber>().Member(dest => dest.SubscriptionPlan, src => src.SubscriptionPlan.ToString());
			Mapper.Register<Subscriber, SubscriberDto>()
				.Member(dest => dest.SubscriptionPlan, src => Enum.Parse(typeof (SubscriptionPlan), src.SubscriptionPlan));

			Mapper.Register<SubscriberDto, User>()
				.Ignore(dest => dest.Roles)
				.Member(dest => dest.UserName, src => src.UserDto.UserName)
				.Member(dest => dest.Password, src => src.UserDto.Password)
				.Member(dest => dest.Email, src => src.UserDto.Email)
				.Member(dest => dest.FirstName, src => src.UserDto.FirstName)
				.Member(dest => dest.LastName, src => src.UserDto.LastName)
				.Member(dest => dest.ValidUntil, src => src.UserDto.ValidUntil);
			//.Member(dest => dest.Subscriber, src => new Subscriber()
			//{
			//	Id = src.Id,
			//	IsSuspended = src.IsSuspended,
			//	IsTrial = src.IsTrial,
			//	StripeId = src.StripeId,
			//	SubscriptionPlan = src.SubscriptionPlan.ToString(),
			//	UserId = src.UserDto.Id
			//})
			//.Member(dest => dest.SubscriberId, src => src.Id);

			Mapper.Register<User, SubscriberDto>()
				.Member(dest => dest.UserDto, src => src);
			//.Member(dest => dest, src => src.Subscriber)
			//.Member(dest => dest.Id, src => src.SubscriberId);
		}
	}
}