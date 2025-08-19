using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.GrpcService;

namespace ChatService.GrpcService
{
    public class UserGrpcClientService
    {
        private readonly UserGrpcService.UserGrpcServiceClient _client;

        public UserGrpcClientService(UserGrpcService.UserGrpcServiceClient client)
        {
            _client = client;
        }

        public async Task<UserResponse?> GetUserByIdAsync(Guid userId)
        {
            var response = await _client.GetUserByIdAsync(new GetUserByIdRequest
            {
                UserId = userId.ToString()
            });
            return response;
        }
    }
}
