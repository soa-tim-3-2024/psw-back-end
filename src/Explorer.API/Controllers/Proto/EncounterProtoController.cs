using Grpc.Net.Client;
using Grpc.Core;
using GrpcServiceTranscoding;
using GrpcServiceTranscodingFol;
using Microsoft.AspNetCore.Authorization;

namespace Explorer.API.Controllers.Proto
{
    public class EncounterProtoController : GrpcServiceTranscoding.Encounters.EncountersBase
    {
        private readonly ILogger<FollowerProtoController> _logger;

        public EncounterProtoController(ILogger<FollowerProtoController> logger)
        {
            _logger = logger;
        }

        [Authorize(Policy = "authorPolicy")]
        public override async Task<SocialEncounterResponse> CreateSocialEncounter(SocialEncounterCreate request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8082", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new GrpcServiceTranscoding.Encounters.EncountersClient(channel);
            var response = await client.CreateSocialEncounterAsync(request);

            return await Task.FromResult(new SocialEncounterResponse(response));
        }

        [Authorize(Policy = "touristPolicy")]
        public override async Task<ListEncounterResponse> GetAllEncounters(Empty request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8082", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new GrpcServiceTranscoding.Encounters.EncountersClient(channel);
            var response = await client.GetAllEncountersAsync(request);

            return await Task.FromResult(new ListEncounterResponse(response));
        }
    }
}
