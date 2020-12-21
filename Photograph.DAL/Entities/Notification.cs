using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photograph.DAL.Entities
{
	public class Notification
	{
		public Guid Id { get; set; }
		public string Text { get; set; }

		public Guid UserId { get; set; }
		public virtual User User { get; set; }
	}
}