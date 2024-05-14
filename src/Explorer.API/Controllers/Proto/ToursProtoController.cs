using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTranscoding;

namespace Explorer.API.Controllers.Proto
{
    public class ToursProtoController : MarketplaceTour.MarketplaceTourBase
    {
        private readonly ILogger<ToursProtoController> _logger;

        public ToursProtoController(ILogger<ToursProtoController> logger)
        {
            _logger = logger;
        }

        public override async Task<TourResponse> GetPublishedTours(Page request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8083", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new MarketplaceTour.MarketplaceTourClient(channel);
            var response = await client.GetPublishedToursAsync(request);

            return await Task.FromResult(new TourResponse
            {
                Id = response.Id,
                ArchiveDate = response.ArchiveDate,
                AuthorId = response.AuthorId,
                AverageRating = response.AverageRating,
                Category = response.Category,
                Description = response.Description,
                Difficulty = response.Difficulty,
                Distance = response.Distance,
                //Durations=response.Durations,
                IsDeleted = response.IsDeleted,
                //KeyPoints =response.KeyPoints,
                Name = response.Name,
                Price = response.Price,
                PublishDate = response.PublishDate,
                Status = response.Status,
                //Tags = response.Tags,
            });
        }

    }
}
