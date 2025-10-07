using Grpc.Core;
using GrpcService;
using NotificationRepository.Model.Request;
using NotificationRepository.Model.Response;
using NotificationService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Implement
{
    public class NotificationGrpcServiceImpl : NotificationGrpcService.NotificationGrpcServiceBase
    {
        private readonly INotificationService _notificationService;

        public NotificationGrpcServiceImpl(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public override async Task<NotificationMessageGrpcResponse> CreateMessageNotification(CreateMessageNotificationGrpcRequest request, ServerCallContext context)
        {
            // gọi logic tạo thông báo
            var result = await _notificationService.CreateMessageNotificationAsync(new CreateMessageNotificationRequest
            {
                ConversationId = Guid.Parse(request.ConversationId.ToString()),
                MessageId = Guid.Parse(request.MessageId.ToString()),
            });
            //trả về response
            return new NotificationMessageGrpcResponse
            {
                Id = result.Id.ToString(),
                UserId = result.UserId.ToString(),
                ConversationId = result.ConversationId.ToString(),
                MessageId = result.MessageId.ToString(),
                Type = result.Type,
                DataJson = result.DataJson ?? "",
                CreatedAt = result.CreatedAt.ToString(),
                IsRead = result.IsRead,
                ConversationName = result.ConversationName,
                ConversationAvatar = result.ConversationAvatar,
                MessageContent = result.MessageContent,
                MessageSentAt = result.MessageSentAt.ToString()
            };

        }
        public override async Task<NotificationMessageGrpcResponse> CreateUserNotification(CreateUserNotificationGrpcRequest request, ServerCallContext context)
        {
            var result = await _notificationService.CreateNotificationForUserAsync(new CreateUserNotificationRequest
            {
                ConversationId = Guid.Parse(request.ConversationId),
                receiverId = Guid.Parse(request.ReceiverId),
                Type = request.Type,
                DataJson = request.DataJson
            });

            return new NotificationMessageGrpcResponse
            {
                Id = result.Id.ToString(),
                UserId = result.UserId.ToString(),
                ConversationId = result.ConversationId.ToString(),
                MessageId = "", // System notification không có messageId
                Type = result.Type,
                DataJson = result.DataJson ?? "",
                CreatedAt = result.CreatedAt.ToString("O"),
                IsRead = result.IsRead,
                ConversationName = result.ConversationName,
                ConversationAvatar = result.ConversationAvatar,
                MessageContent = "",
                MessageSentAt = ""
            };
        }
    }
}
