using Grpc.Core;
using GrpcService;
using UserRepository.Repositories;
using UserService;

public class UserGrpcServiceImpl : UserGrpcService.UserGrpcServiceBase
{
    private readonly IUserRepository _userRepository;

    public UserGrpcServiceImpl(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task<UserReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Id) || !Guid.TryParse(request.Id, out var userId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid user id format"));
        }

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with id {userId} not found"));
        }

        return new UserReply
        {
            Id = user.Id.ToString(),
            DisplayName = user.DisplayName ?? string.Empty,
            AvatarUrl = user.AvatarUrl ?? string.Empty
        };
    }
}
