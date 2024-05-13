using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTranscoding;

namespace Explorer.API.Controllers.Proto
{
    public class KeyPointProtoController : AuthoringKeyPoint.AuthoringKeyPointBase
    {
        private readonly ILogger<KeyPointProtoController> _logger;

        public KeyPointProtoController(ILogger<KeyPointProtoController> logger)
        {
            _logger = logger;
        }

        public override async Task<KeyPoint> CreateKeyPoint(KeyPointCreate request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8083", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new AuthoringKeyPoint.AuthoringKeyPointClient(channel);
            var response = await client.CreateKeyPointAsync(request);

            return await Task.FromResult(response);
        }
        public override async Task<KeyPoint> UpdateKeyPoint(KeyPointUpdate request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8083", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new AuthoringKeyPoint.AuthoringKeyPointClient(channel);
            var response = await client.UpdateKeyPointAsync(request);

            return await Task.FromResult(response);
        }

        public override async Task<KeyPoint> DeleteKeyPoint(KeyPointId request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8083", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new AuthoringKeyPoint.AuthoringKeyPointClient(channel);
            var response = await client.DeleteKeyPointAsync(request);

            return await Task.FromResult(response);
        }
    }
}
