using ChatRepository.Model.Request;
using ChatRepository.Model.Response;
using ChatRepository.Models;
using ChatRepository.Repositories;
using GrpcService;

namespace ChatService.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly UserGrpcService.UserGrpcServiceClient _userGrpcClient;
        public ConversationService(IConversationRepository conversationRepository, UserGrpcService.UserGrpcServiceClient userGrpcServiceClient)
        {
            _conversationRepository = conversationRepository;
            _userGrpcClient = userGrpcServiceClient;
        }

        //public async Task<Conversations> CreateConversationAsync(Conversations conversation, Guid creatorId, List<Guid> participantIds)
        //{
        //    //neu la chat rieng tu 1-1
        //    if(!conversation.IsGroup && conversation.IsPrivate)
        //    {

                
        //        if (participantIds.Count != 1)
        //            throw new InvalidOperationException("Private conversation must be exactly between 2 people.");
        //        Guid otherUserId = participantIds.First();
        //        //tìm xem giữa 2 ng có đoạn chat hay ko
        //        var existing = await _conversationRepository.GetUserConversationsAsync(creatorId);
        //        var existingPrivate = existing.FirstOrDefault(c =>
        //        !c.IsGroup &&
        //        c.IsPrivate &&
        //        c.Participants.Count == 2 &&
        //        c.Participants.Any(p => p.UserId == creatorId) &&
        //        c.Participants.Any(p => p.UserId == otherUserId)

        //        );
        //        //nếu có thì sử dụng 
        //        if(existingPrivate != null)
        //        {
        //            return existingPrivate;
        //        }
        //        //nếu chưa có thì tạo mới
        //        conversation.Participants.Add(new Participants
        //        {
        //            UserId = creatorId,
        //            JoinAt = DateTime.UtcNow
        //        });
        //        conversation.Participants.Add(new Participants
        //        {
        //            UserId = otherUserId,
        //            JoinAt = DateTime.UtcNow
        //        });
        //        await _conversationRepository.AddConversationAsync(conversation);
        //        return conversation;

        //    }
        //    if (conversation.IsGroup)
        //    {
        //        // thêm người tạo nhóm vào
        //        conversation.AdminId = creatorId;
        //        conversation.Participants.Add(new Participants
        //        {
        //            UserId = creatorId,
        //            JoinAt = DateTime.UtcNow
        //        });
        //        // Nếu chưa có thì tạo mới foreach (var userId in participantIds)
        //        foreach (var userId in participantIds)
        //        {
        //            conversation.Participants.Add(new Participants
        //            {
        //                UserId = userId,
        //                ConversationId = conversation.Id,
        //                JoinAt = DateTime.UtcNow
        //            });
        //        }
        //        await _conversationRepository.AddConversationAsync(conversation);
        //        return conversation;
        //    }
        //    throw new InvalidOperationException("Invalid conversation type.");
        //}

        public async Task<ConversationResponse> CreateConversationAsync(ConversationCreateRequest request, Guid creatorId)
        {
            var conversation = new Conversations
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                IsGroup = request.IsGroup,
                IsPrivate = request.IsPrivate,
                CreatedAt = DateTime.UtcNow,
                Participants = new List<Participants>()
            };
            // Nếu là private 1-1
            if (!conversation.IsGroup && conversation.IsPrivate)
            {
                if (request.ParticipantIds.Count != 1)
                    throw new InvalidOperationException("Private conversation must be exactly between 2 people.");

                Guid otherUserId = request.ParticipantIds.First();

                // kiểm tra đã có chưa
                var existing = await _conversationRepository.GetUserConversationsAsync(creatorId);
                var existingPrivate = existing.FirstOrDefault(c =>
                    !c.IsGroup &&
                    c.IsPrivate &&
                    c.Participants.Count == 2 &&
                    c.Participants.Any(p => p.UserId == creatorId) &&
                    c.Participants.Any(p => p.UserId == otherUserId));

                if (existingPrivate != null)
                {
                    return await MapToResponse(existingPrivate, creatorId);
                }

                // nếu chưa có thì tạo mới
                conversation.Participants.Add(new Participants
                {
                    UserId = creatorId,
                    JoinAt = DateTime.UtcNow
                });
                conversation.Participants.Add(new Participants
                {
                    UserId = otherUserId,
                    JoinAt = DateTime.UtcNow
                });

                await _conversationRepository.AddConversationAsync(conversation);
                return await MapToResponse(conversation, creatorId);
            }
            // nếu là group
            if (conversation.IsGroup)
            {
                conversation.AdminId = creatorId;
                conversation.Participants.Add(new Participants
                {
                    UserId = creatorId,
                    JoinAt = DateTime.UtcNow
                });

                foreach (var userId in request.ParticipantIds)
                {
                    conversation.Participants.Add(new Participants
                    {
                        UserId = userId,
                        JoinAt = DateTime.UtcNow
                    });
                }

                await _conversationRepository.AddConversationAsync(conversation);
                return await MapToResponse(conversation, creatorId);
            }

            throw new InvalidOperationException("Invalid conversation type.");

        }

        public async Task DeleteConversationAsync(Guid id)
        {
            await _conversationRepository.DeleteConversationAsync(id);
        }

        //public async Task<Conversations?> GetConversationByIdAsync(Guid id)
        //{
        //   return await _conversationRepository.GetConversationByIdAsync(id);
        //}

        //public async Task<IEnumerable<Conversations>> GetUserConversationsAsync(Guid userId)
        //{
        //   return await _conversationRepository.GetUserConversationsAsync(userId);
        //}

        public async Task<IEnumerable<ConversationResponse>> SearchConversationsAsync(Guid userId, string conversationName)
        {
            var conversations = await _conversationRepository.SearchConversationsAsync(userId, conversationName);
            return await Task.WhenAll(conversations.Select(c => MapToResponse(c, userId)));
        }

        public async Task UpdateConversationAsync(Guid id,ConversationUpdateRequest request, Guid adminGroupId)
        {
            var conversation = await _conversationRepository.GetConversationByIdAsync(id);
            if (conversation == null) throw new KeyNotFoundException("Conversation not found.");
            if (conversation.AdminId != adminGroupId) throw new UnauthorizedAccessException("Only admin can update conversation.");

            conversation.Name = request.Name ?? conversation.Name;
            conversation.IsPrivateGroup = request.IsPrivateGroup;
            conversation.AdminId = request.AdminId ?? conversation.AdminId;
            await _conversationRepository.UpdateConversationAsync(conversation);
        }

        public async Task<ConversationResponse?> GetConversationByIdAsync(Guid id)
        {
            var conversation = await _conversationRepository.GetConversationByIdAsync(id);
            if (conversation == null) return null;

            return await MapToResponse(conversation, Guid.Empty);
        }

        public async Task<IEnumerable<ConversationResponse>> GetUserConversationsAsync(Guid userId)
        {
            var conversations = await _conversationRepository.GetUserConversationsAsync(userId);
            var responses = new List<ConversationResponse>();
            foreach (var c in conversations)
            {
                responses.Add(await MapToResponse(c, userId));
            }
            return responses;
        }

        // ✅ MapToResponse: gọi sang gRPC để lấy thông tin user
        public async Task<ConversationResponse> MapToResponse(Conversations conversation, Guid currentUserId)
        {
            var response = new ConversationResponse
            {
                Id = conversation.Id,
                Name = conversation.Name,
                IsGroup = conversation.IsGroup,
                IsPrivate = conversation.IsPrivate,
                IsPrivateGroup =conversation.IsPrivateGroup,
                AdminId = conversation.AdminId,
                CreatedAt = conversation.CreatedAt,
                Participants = new List<ParticipantResponse>()
            };

            foreach (var p in conversation.Participants)
            {
                var grpcReply = await _userGrpcClient.GetUserByIdAsync(new GetUserByIdRequest
                {
                    Id = p.UserId.ToString()
                });

                response.Participants.Add(new ParticipantResponse
                {
                    UserId = Guid.Parse(grpcReply.Id),
                    JoinAt = p.JoinAt,
                    DisplayName = grpcReply.DisplayName,
                    AvatarUrl = grpcReply.AvatarUrl
                });
            }

            // Nếu là chat 1-1 → đổi tên thành tên user kia
            if (!conversation.IsGroup && conversation.Participants.Count == 2)
            {
                var otherUser = response.Participants.FirstOrDefault(u => u.UserId != currentUserId);
                if (otherUser != null)
                {
                    response.Name = otherUser.DisplayName;
                }
            }

            return response;
        }

    }
}
