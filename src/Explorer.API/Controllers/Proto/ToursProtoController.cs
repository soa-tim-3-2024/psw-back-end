using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTranscoding;
using Microsoft.AspNetCore.Authorization;

namespace Explorer.API.Controllers.Proto
{

    public class ToursProtoController : MarketplaceTour.MarketplaceTourBase
    {
        private readonly ILogger<ToursProtoController> _logger;

        public ToursProtoController(ILogger<ToursProtoController> logger)
        {
            _logger = logger;
        }

        [Authorize(Policy = "touristPolicy")]
        public override async Task<TourResponseList> GetPublishedTours(Page request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8083", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new MarketplaceTour.MarketplaceTourClient(channel);
            var response = await client.GetPublishedToursAsync(request);
            
            return await Task.FromResult(response);
        }

        [Authorize(Policy = "authorPolicy")]
        public override async Task<TourResponseList> GetAuthorTours(AuthorId request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8083", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new MarketplaceTour.MarketplaceTourClient(channel);
            var response = await client.GetAuthorToursAsync(request);
            
            return await Task.FromResult(response);
        }

        public override async Task<TourResponse> GetTour(TourId request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8083", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new MarketplaceTour.MarketplaceTourClient(channel);
            var response = await client.GetTourAsync(request);

            return await Task.FromResult(response);
        }
    }
}
