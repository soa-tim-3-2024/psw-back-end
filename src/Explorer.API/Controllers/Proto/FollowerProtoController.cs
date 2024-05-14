using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTranscoding;
using GrpcServiceTranscodingFol;

namespace Explorer.API.Controllers.Proto
{
    public class FollowerProtoController : Followers.FollowersBase
    {
        private readonly ILogger<FollowerProtoController> _logger;

        public FollowerProtoController(ILogger<FollowerProtoController> logger)
        {
            _logger = logger;
        }

        public override async Task<ListFollowingResponse> GetUserFollowings(Identificator request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8089", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Followers.FollowersClient(channel);
            var response = await client.GetUserFollowingsAsync(request);

            return await Task.FromResult(new ListFollowingResponse(response));
        }

        public override async Task<ListFollowingResponse> GetUserFollowers(Identificator request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8089", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Followers.FollowersClient(channel);
            var response = await client.GetUserFollowersAsync(request);

            return await Task.FromResult(new ListFollowingResponse(response));
        }

        public override async Task<ListFollowingResponse> GetUserRecommendations(Identificator request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8089", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Followers.FollowersClient(channel);
            var response = await client.GetUserRecommendationsAsync(request);

            return await Task.FromResult(new ListFollowingResponse(response));
        }

        public override async Task<FollowerResponse> CreateNewFollowing(FollowingCreateRequest request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8089", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Followers.FollowersClient(channel);
            var response = await client.CreateNewFollowingAsync(request);

            return await Task.FromResult(new FollowerResponse(response));
        }

        public override async Task<FollowerResponse> UnfollowUser(UserUnfollowRequest request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8089", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Followers.FollowersClient(channel);
            var response = await client.UnfollowUserAsync(request);

            return await Task.FromResult(new FollowerResponse(response));
        }

    }
}
