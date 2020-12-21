using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository.Ports;

namespace Photograph.BLL.Services.IdentityServerServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<InMemoryUser> GetUser(string userName, string password)
        {
            var user = (await _userRepository.GetAllAsync()).SingleOrDefault(
                x => x.UserName.Equals(userName));
            if (user == null || !user.Password.Equals(password)) return null;

            var tempUser = new InMemoryUser()
            {
                Subject = user.Id.ToString(),
                Claims = ExtractClaimsList(user),
                Username = user.UserName,
                Password = user.Password,
            };

            return tempUser;
        }

        public async Task<InMemoryUser> GetUser(string id)
        {
            var user = await _userRepository.GetByIdAsync(Guid.Parse(id));

            if (user == null) return null;

            var tempUser = new InMemoryUser()
            {
                Subject = user.Id.ToString(),
                Claims = ExtractClaimsList(user),
                Username = user.UserName,
                Password = user.Password,
            };

            return tempUser;
        }

        private static IEnumerable<Claim> ExtractClaimsList(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(Constants.ClaimTypes.Id, user.Id.ToString()),
                new Claim("username", user.UserName),
                new Claim(Constants.ClaimTypes.Email, user.Email),
				new Claim("valid_until", user.ValidUntil.ToString()),
				new Claim("isSuspended", user.IsSuspended.ToString())
			};
            claims.AddRange(user.Roles.Select(roleDto => new Claim(Constants.ClaimTypes.Role, roleDto.Name)).ToList());

			return claims;
        }
    }
}