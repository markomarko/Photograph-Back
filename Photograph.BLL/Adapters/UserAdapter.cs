using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressMapper;
using ExpressMapper.Extensions;
using Photograph.BLL.Dtos;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository.Adapters;

namespace Photograph.BLL.Adapters
{
	public class UserAdapter
	{
		public static UserDto BuildUserDto(User user)
		{
			return user.Map<User, UserDto>();
		}

		public static User BuildUser(UserDto userDto)
		{
			return userDto.Map<UserDto, User>();
		}

		public static SubscriberDto BuildSubscriberDto(User user)
		{
			return user.Map<User, SubscriberDto>();
		}

		public static User BuildUser(SubscriberDto userDto)
		{
			return userDto.Map<SubscriberDto, User>();
		}
	}
}