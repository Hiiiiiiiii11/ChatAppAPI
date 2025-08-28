using NotificationRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetALlNotificationsAsync();
        Task<Notification?> GetNotificationByIdAsync(Guid id);
        Task AddNotificationAsync(Notification notification);
        Task UpdateNotificationAsync(Notification notification);
        Task DeleteNotificationAsync(Guid id);
        Task MarkAsReadAsync(Guid id);
    }
}
