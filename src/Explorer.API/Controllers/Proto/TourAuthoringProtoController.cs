using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTranscoding;

namespace Explorer.API.Controllers.Proto
{
    public class TourAuthoringProtoController : Authoring.AuthoringBase
    {
        private readonly ILogger<TourAuthoringProtoController> _logger;

        public TourAuthoringProtoController(ILogger<TourAuthoringProtoController> logger)
        {
            _logger = logger;
        }

        public override async Task<TourResponseAuthor> AddTour(TourCreate request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8083", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Authoring.AuthoringClient(channel);
            var response = await client.AddTourAsync(request);
            
            return await Task.FromResult(new TourResponseAuthor
            {
                Id = response.Id,
                ArchiveDate = response.ArchiveDate,
                AuthorId = response.AuthorId,
                AverageRating = response.AverageRating,
                Description = response.Description,
                Difficulty = response.Difficulty,
                Distance = response.Distance,
                //Durations=response.Durations,
                IsDeleted = response.IsDeleted,
                //KeyPoints =response.KeyPoints,
                Name = response.Name,
                Price = response.Price,
                PublishDate = response.PublishDate
                //Tags = response.Tags,
            });
        }

        public override async Task<TourResponseAuthor> UpdateTour(TourUpdate request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8083", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Authoring.AuthoringClient(channel);
            var response = await client.UpdateTourAsync(request);

            return await Task.FromResult(new TourResponseAuthor
            {
                Id = response.Id,
                ArchiveDate = response.ArchiveDate,
                AuthorId = response.AuthorId,
                AverageRating = response.AverageRating,
                Description = response.Description,
                Difficulty = response.Difficulty,
                Distance = response.Distance,
                //Durations=response.Durations,
                IsDeleted = response.IsDeleted,
                //KeyPoints =response.KeyPoints,
                Name = response.Name,
                Price = response.Price,
                PublishDate = response.PublishDate
                //Tags = response.Tags,
            });
        }

        public override async Task<publishResponse> PublishTour(TourUpdate request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8083", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Authoring.AuthoringClient(channel);
            var response = await client.PublishTourAsync(request);

            return await Task.FromResult(response);
        }
        public override async Task<publishResponse> ArchiveTour(TourUpdate request, ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://localhost:8083", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Authoring.AuthoringClient(channel);
            var response = await client.ArchiveTourAsync(request);

            return await Task.FromResult(response);
        }
    }
}
