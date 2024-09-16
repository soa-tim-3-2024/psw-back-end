using Explorer.API.Controllers.Author;
using Explorer.API.Controllers.Tourist.TourExecution;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.TouristPosition;
using Explorer.Tours.API.Public;
using Explorer.Tours.API.Public.TourExecution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration;

[Collection("Sequential")]
public class TouristPositionQueryTests : BaseToursIntegrationTest
{
    public TouristPositionQueryTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public async Task Retrieves_for_single_touristAsync()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = (await controller.GetByTouristId(-1))?.Value as TouristPositionResponseDto;

        // Assert
        result.ShouldNotBeNull();
    }

    private static TouristPositionController CreateController(IServiceScope scope)
    {
        return new TouristPositionController(scope.ServiceProvider.GetRequiredService<ITouristPositionService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}