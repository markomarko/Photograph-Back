using System;
using System.Collections.Generic;
using Photograph.BLL.Dtos;

namespace Photograph.BLL.Services
{
	public interface INotificationService
	{
		List<NotificationDto> GetAll(Guid id);
		void Save(NotificationDto notification);
	}
}