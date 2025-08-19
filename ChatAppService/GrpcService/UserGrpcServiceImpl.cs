using Grpc.Core;
using UserService.GrpcService;
using UserService.Repositories;

namespace UserService.GrpcService
{
    public class UserGrpcServiceImpl : UserGrpcService.UserGrpcServiceBase
    {
        private readonly IUserRepository _userRepository;

        public UserGrpcServiceImpl(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<UserResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            var user = await _userRepository.GetByIdAsync(Guid.Parse(request.UserId));
            if (user == null)
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

            return new UserResponse
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                DisplayName = user.DisplayName,
                AvatarUrl = user.AvatarUrl ?? "",
                IsActive = user.IsActive
            };
        }

        public override async Task<UserResponse> GetUserByEmail(GetUserByEmailRequest request, ServerCallContext context)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

            return new UserResponse
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                DisplayName = user.DisplayName,
                AvatarUrl = user.AvatarUrl ?? "",
                IsActive = user.IsActive
            };
        }
    }
}
