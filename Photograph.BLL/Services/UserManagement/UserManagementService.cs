using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.BLL.Adapters;
using Photograph.BLL.Dtos;
using Photograph.BLL.Services.MailService;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository;
using Photograph.DAL.Repository.Ports;

namespace Photograph.BLL.Services.UserManagement
{
	public class UserManagementService : IUserManagementService
	{
		private readonly IUserRepository _userRepository;
		private readonly IGenericRepository<User, Guid> _gUserRepository;
		private readonly IGenericRepository<Subscriber, Guid> _gSubscriberRepository;
		private readonly IGenericRepository<Role, Guid> _roleRepository;
		private readonly IGenericRepository<Photo, Guid> _photoRepository;
		private readonly IEmailService _email;

		public UserManagementService(IUserRepository userRepository, IGenericRepository<Role, Guid> roleRepository,
			IGenericRepository<Photo, Guid> photoRepository, IEmailService email, IGenericRepository<User, Guid> gUserRepository,
			IGenericRepository<Subscriber, Guid> gSubscriberRepository)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
			_photoRepository = photoRepository;
			_email = email;
			_gUserRepository = gUserRepository;
			_gSubscriberRepository = gSubscriberRepository;
		}

		public UserDto GetUser(Guid id, Guid requestorId)
		{
			var userRequestor = _userRepository.GetUserById(id);
			if (userRequestor.Roles[0].Name.Equals(RoleConstants.Admin))
			{
				return UserAdapter.BuildUserDto(_userRepository.GetUserById(id));
			}
			return UserAdapter.BuildUserDto(_userRepository.GetUserById(requestorId));
		}

		public IEnumerable<UserDto> GetAll(Guid id, PagingParameterDto paging)
		{
			var start = (paging.pageNumber - 1)*paging._pageSize;

			var userRequestor = _userRepository.GetUserById(id);

			if (userRequestor.Roles[0].Name.Equals(RoleConstants.Admin))
			{
				return (_gUserRepository.GetRange(start, paging._pageSize, x => x.Id))
					.Select(UserAdapter.BuildUserDto);
			}

			var subscriber = _gSubscriberRepository.Get(id);
			return subscriber.DependentUsers.Count > 0
				? subscriber.DependentUsers
					.Skip(start)
					.Take(paging._pageSize)
					.Select(UserAdapter.BuildUserDto)
				: new List<UserDto>();
		}

        public IEnumerable<UserDto> GetAll(Guid id)
        {
           // var userRequestor = _userRepository.GetUserById(id);
	        var subscriber = _gSubscriberRepository.Get(id);
	        return subscriber.DependentUsers.Select(UserAdapter.BuildUserDto);

			//if (userRequestor.Roles[0].Name.Equals(RoleConstants.Subscriber))
   //         {
   //             var userList = _gUserRepository.GetAll();

   //             if (userList != null)
   //             {
   //                 List<UserDto> tempList = new List<UserDto>();
   //                 foreach (var user in userList)
   //                 {
   //                    if(user.DependsOnAdmin!=null)
   //                     {
   //                         foreach(var u in user.DependsOnAdmin)
   //                         {
   //                             if (u.UserId.Equals(userRequestor.Id))
   //                                 tempList.Add(UserAdapter.BuildUserDto(user));
   //                         }
   //                     }
   //                 }
   //                 return tempList;
   //             }                
   //         }
   //         return null;
        }

        public int CountDependedUsers(Guid id)
		{
			var userRequestor = _userRepository.GetUserById(id);

			var userCount = userRequestor.Roles[0].Name.Equals(RoleConstants.Admin)
				? _gUserRepository.Count()
				: _gSubscriberRepository.Get(id).DependentUsers.Count;

			return userCount;
		}

		public void Suspend(Guid id)
		{
			var suspendedUser = _gUserRepository.Get(id);
			suspendedUser.IsSuspended = true;
			_gUserRepository.Update(suspendedUser);
		}

		public void Resume(Guid id)
		{
			var suspendedUser = _gUserRepository.Get(id);
			suspendedUser.IsSuspended = false;
			_gUserRepository.Update(suspendedUser);
		}

		public UserDto GetUserByEmail(string subscriberEmail)
		{
			return UserAdapter.BuildUserDto(_gUserRepository.Find(usr => usr.Email.Equals(subscriberEmail)).FirstOrDefault());
		}

		public void Create(string email, UserDto userDto)
		{
			var user = UserAdapter.BuildUser(userDto);

			var sub = _gUserRepository.Find(usr => usr.Email.Equals(email)).FirstOrDefault();

			if (sub != null)
			{
				var subs = _userRepository.GetUserById(sub.Id).Subscriber;

				user.DependsOnAdmin = new List<Subscriber>() { subs };
			}

			var roleUser = _roleRepository.Find(x => x.Name.Equals(RoleConstants.User)).FirstOrDefault();
			user.Roles = new List<Role> {roleUser};

			_userRepository.AddUser(user);

			var body = _email.CreateEmailBody(user.UserName, "Subscriber");
			_email.SendEmail("Welcome", body, user.Email);
		}

		public void Create(string tokenId, SubscriberDto subscriberDto)
		{
			var user = UserAdapter.BuildUser(subscriberDto);
			user.Id = Guid.NewGuid();

			var roleUser = _roleRepository.Find(x => x.Name.Equals(RoleConstants.Subscriber)).FirstOrDefault();

			user.Roles = new List<Role> {roleUser};

			var customerId = StripeService.StripeService.CreateCustomer(tokenId, user.Email);

			var subscriberId = StripeService.StripeService.SubscribeCustomer(customerId, subscriberDto.SubscriptionPlan);

			var sub = new Subscriber
			{
				UserId = user.Id,
				StripeId = customerId,
				SubscriptionPlan = subscriberDto.SubscriptionPlan.ToString(),
				IsTrial = subscriberDto.IsTrial
			};

			_userRepository.AddUser(user);

			_gSubscriberRepository.Add(sub);

			var body = _email.CreateEmailBody(user.UserName, "Subscriber");
			_email.SendEmail("Welcome", body, user.Email);
		}

		public void Edit(UserDto userDto, Guid requestorUserId)
		{
			var user = UserAdapter.BuildUser(userDto);

			var userRequestor = _userRepository.GetUserById(requestorUserId);

			if (userRequestor.Roles[0].Name.Equals(RoleConstants.Subscriber))
			{
				var roleUser = _roleRepository.Find(x => x.Name.Equals(RoleConstants.User)).FirstOrDefault();
				user.Roles = new List<Role> {roleUser};
				user.ValidUntil = userRequestor.ValidUntil;
			}
			else
			{
				var role = _roleRepository.Find(x => x.Name.Equals(userDto.Roles[0])).FirstOrDefault();
				user.Roles = new List<Role> {role};
			}

			_userRepository.UpdateUser(user);
		}

		public void Delete(Guid id, Guid requestorUserId)
		{
			var user = _userRepository.GetUserById(id);
			var userRequestor = _userRepository.GetUserById(requestorUserId);

			if (user.DependsOnAdmin.Any(subscriber => subscriber.UserId.Equals(requestorUserId)) ||
				userRequestor.Roles[0].Name.Equals(RoleConstants.Admin))
			{
				var photos = _photoRepository.Find(photo => photo.UserId.Equals(id));

				photos.ToList().ForEach(photo =>
				{
					var dbPhoto = _photoRepository.Get(photo.Id);

					if (dbPhoto != null) _photoRepository.Remove(dbPhoto);
				});

				_userRepository.DeleteUser(id);
			}
		}

		public bool IsUsernameAvailable(string username)
		{
			return !_gUserRepository.Find(user => user.UserName.Equals(username)).Any();
		}
	}
}