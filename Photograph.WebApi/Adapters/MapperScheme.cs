using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressMapper;
using Photograph.BLL.Dtos;
using Photograph.WebApi.Models;

namespace Photograph.WebApi.Adapters
{
	public class MapperScheme
	{
		public static void RegisterMapping()
		{
			Photograph.BLL.Shared.MapperScheme.RegisterMapping();
			Mapper.Register<UserDto, UserViewModel>().Ignore(dest => dest.Password);
		    Mapper.Register<UserViewModel, UserDto>().Member(dest => dest.Id, src => Guid.NewGuid());

			Mapper.Register<PhotoDto, PhotoViewModel>();
			Mapper.Register<PhotoViewModel, PhotoDto>();

			Mapper.Register<AlbumDto, AlbumViewModel>();
			Mapper.Register<AlbumViewModel, AlbumDto>();

			Mapper.Register<PagingParameterModel, PagingParameterDto>();
			Mapper.Register<PagingParameterDto, PagingParameterModel>();

			Mapper.Register<SubscriberDto, SubscriberViewModel>()
				.Member(dest => dest.SubscriptionPlan, src => src.SubscriptionPlan.ToString())
				.Member(dest => dest.SubscriptionPlan, src => src.Id.ToString());
			Mapper.Register<SubscriberViewModel, SubscriberDto>()
				.Ignore(dest => dest.Id)
				.Member(dest => dest.SubscriptionPlan, src => (SubscriptionPlan)Enum.Parse(typeof (SubscriptionPlan), src.SubscriptionPlan))
				.Member(dest => dest.UserDto, src => new UserDto()
				{
					UserName = src.UserName,
					FirstName = src.FirstName,
					LastName = src.LastName,
					Email = src.Email,
					Password = src.Password,
					Roles = new List<string>() {"Subscriber"},
					ValidUntil = GetValidTime(src.SubscriptionPlan)
				});
		}

		private static double GetValidTime(string subscriptionPlan)
		{
			double validTime = 0;
			switch (subscriptionPlan)
			{
				case "TRIAL":
					validTime = ToUnixTimeSeconds(DateTime.UtcNow.AddDays(20));
					break;

				case "MONTH":
					validTime = ToUnixTimeSeconds(DateTime.UtcNow.AddMonths(1));

					break;
				case "HALF_YEAR":
					validTime = ToUnixTimeSeconds(DateTime.UtcNow.AddMonths(6));

					break;
				case "YEAR":
					validTime = ToUnixTimeSeconds(DateTime.UtcNow.AddYears(1));

					break;
			}
			return validTime;
		}

		private static double ToUnixTimeSeconds(DateTime dateTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var unixDateTime = (dateTime.ToUniversalTime() - epoch).TotalSeconds;

			return unixDateTime;
		}
	}
}