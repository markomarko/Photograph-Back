using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photograph.BLL.Dtos
{
	public class UserDto
	{
		public Guid Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public double ValidUntil { get; set; }
		public bool IsSuspended { get; set; }
        public List<string> Roles { get; set; }
	}
}