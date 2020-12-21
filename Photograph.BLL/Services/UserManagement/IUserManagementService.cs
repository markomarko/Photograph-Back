using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.BLL.Dtos;
using Photograph.DAL.Entities;

namespace Photograph.BLL.Services.UserManagement
{
	public interface IUserManagementService
	{
		UserDto GetUser(Guid id, Guid requestorId);

		IEnumerable<UserDto> GetAll(Guid id, PagingParameterDto buildPagingDto);
        IEnumerable<UserDto> GetAll(Guid id);

        void Create(string email, UserDto userDto);
		void Create(string tokenId, SubscriberDto subscriberDto);

		void Edit(UserDto userDto, Guid requestorUserId);

		void Delete(Guid id, Guid requestorUserId);
		bool IsUsernameAvailable(string username);

		int CountDependedUsers(Guid id);
		void Suspend(Guid id);
		void Resume(Guid id);
		UserDto GetUserByEmail(string subscriberEmail);
	}
}