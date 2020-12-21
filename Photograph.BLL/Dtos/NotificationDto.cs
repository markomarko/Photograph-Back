using System;

namespace Photograph.BLL.Dtos
{
	public class NotificationDto
	{
		public Guid Id { get; set; }
		public string Text { get; set; }

		public Guid UserId { get; set; }
	}
}