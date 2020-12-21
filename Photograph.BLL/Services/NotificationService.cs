using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.BLL.Adapters;
using Photograph.BLL.Dtos;
using Photograph.DAL.Entities;
using Photograph.DAL.Repository;

namespace Photograph.BLL.Services
{
	public class NotificationService : INotificationService
	{
		private readonly IGenericRepository<Notification, Guid> _notificationRepository;

		public NotificationService(IGenericRepository<Notification, Guid> notificationRepository)
		{
			_notificationRepository = notificationRepository;
		}

		public List<NotificationDto> GetAll(Guid id)
		{
			return _notificationRepository.Find(x => x.UserId.Equals(id))
				.Select(NotificationAdapter.BuildNotificationDto)
				.ToList();
		}

		public void Save(NotificationDto notification)
		{
			notification.Id = Guid.NewGuid();
			_notificationRepository.Add(NotificationAdapter.BuildNotification(notification));
		}
	}
}