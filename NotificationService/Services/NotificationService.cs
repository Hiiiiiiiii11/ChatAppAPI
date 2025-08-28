
using NotificationRepository.Models;
using NotificationService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task AddNotificationAsync(Notification notification)
        {
            notification.Id = Guid.NewGuid();
            notification.CreatedAt = DateTime.UtcNow;
            await _notificationRepository.AddAsync(notification);
        }

        public async Task DeleteNotificationAsync(Guid id)
        {
            await _notificationRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetALlNotificationsAsync()
        {
            return await _notificationRepository.GetAllAsynnc();
        }

        public async Task<Notification?> GetNotificationByIdAsync(Guid id)
        {
            return await _notificationRepository.GetByIdAsync(id);
        }

        public Task MarkAsReadAsync(Guid id)
        {
            return _notificationRepository.UpdateAsync(new Notification
            {
                Id = id,
                IsRead = true
            });
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            await _notificationRepository.UpdateAsync(notification);
        }
    }
}
