using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressMapper.Extensions;
using Photograph.BLL.Dtos;
using Photograph.WebApi.Models;

namespace Photograph.WebApi.Adapters
{
    public class UserViewModelAdapter
    {
        public static UserViewModel BuiUserViewModel(UserDto userDto)
        {
            return userDto.Map<UserDto, UserViewModel>();
        }

        public static UserDto BuildUserDto(UserViewModel userViewModel)
        {
            return userViewModel.Map<UserViewModel, UserDto>();
        }
    }
}