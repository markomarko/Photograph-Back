using ExpressMapper.Extensions;
using Photograph.BLL.Dtos;
using Photograph.DAL.Entities;

namespace Photograph.BLL.Adapters
{
	public class NotificationAdapter
	{
		public static NotificationDto BuildNotificationDto(Notification obj)
		{
			return obj.Map<Notification, NotificationDto>();
		}

		public static Notification BuildNotification(NotificationDto obj)
		{
			return obj.Map<NotificationDto, Notification>();
		}
	}
}
