using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Photograph.DAL.Entities;

namespace Photograph.DAL.Repository.Ports
{
	public interface IUserRepository
	{
		User GetUserById(Guid id);
		Task<User> GetByIdAsync(Guid id);
		Task<IEnumerable<User>> GetAllAsync();
		IEnumerable<User> GetAllUsers();
		void AddUser(User user);
		void UpdateUser(User user);
		void DeleteUser(Guid id);
	}
}