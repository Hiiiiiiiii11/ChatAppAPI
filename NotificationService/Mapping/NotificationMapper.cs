using NotificationRepository.Model.Response;
using NotificationRepository.Models;
using System;
using System.Text.Json;

namespace NotificationService.Mapping
{
    public static class NotificationMapper
    {
        // Map entity -> NotificationResponse (dùng cho các API chung chung)
        public static NotificationResponse ToResponse(this Notification notification)
        {
            return new NotificationResponse
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Type = notification.Type,
                DataJson = notification.DataJson,
                CreatedAt = notification.CreatedAt,
                IsRead = notification.IsRead,

            };
        }

        // Map entity -> NotificationMessageResponse (message notification, có context gRPC)
        public static NotificationMessageResponse ToMessageResponse(this Notification notification)
        {
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(notification.DataJson ?? "{}");

            return new NotificationMessageResponse
            {
                Id = notification.Id,
                UserId = notification.UserId,
                ConversationId = notification.ConversationId,
                MessageId = notification.MessageId,
                Type = notification.Type,
                DataJson = notification.DataJson,
                CreatedAt = notification.CreatedAt,
                IsRead = notification.IsRead,
                ConversationName = data!.GetValueOrDefault("ConversationName", ""),
                ConversationAvatar = data!.GetValueOrDefault("ConversationAvatar", ""),
                MessageContent = data!.GetValueOrDefault("MessageContent", ""),
                MessageSentAt = DateTime.TryParse(data!.GetValueOrDefault("SentAt", ""), out var sentAt)
                                ? sentAt
                                : notification.CreatedAt
            };
        }
    }
}
