using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository.Ports;

namespace Photograph.DAL.Repository.Adapters
{
	public class RoleRepository : GenericRepository<Role, Guid>
	{
		public RoleRepository(DbContext context) : base(context)
		{
		}
	}
}