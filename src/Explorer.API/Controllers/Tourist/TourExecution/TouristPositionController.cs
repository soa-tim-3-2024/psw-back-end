using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.TouristPosition;
using Explorer.Tours.API.Public.TourExecution;
using Explorer.Tours.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Tourist.TourExecution;

[Authorize(Policy = "touristPolicy")]
[Route("api/tour-execution/tourists")]
public class TouristPositionController : BaseApiController
{
    private readonly ITouristPositionService _touristPositionService;
    private readonly IHttpClientFactory _factory;

    public TouristPositionController(ITouristPositionService touristPositionService, IHttpClientFactory factory)
    {
        _touristPositionService = touristPositionService;
        _factory = factory;
    }

    [HttpPost("position")]
    public async Task<ActionResult<TouristPositionResponseDto>> Create([FromBody] TouristPositionCreateDto touristPosition)
    {
        var client = _factory.CreateClient();
        /*var result = _touristPositionService.Create(touristPosition);
        return CreateResponse(result);
        */
        var position = await AddPositionGo(client, touristPosition);
        return position;
    }

    [HttpPut("position")]
    public async Task<ActionResult<TouristPositionResponseDto>> Update([FromBody] TouristPositionUpdateDto touristPosition)
    {
        /*var result = _touristPositionService.Update(touristPosition);
        return CreateResponse(result);*/
        var client = _factory.CreateClient();
        var position = await UpdatePositionGo(client, touristPosition);
        return position;
    }

    [HttpGet("{touristId:long}/position")]
    public async Task<ActionResult<TouristPositionResponseDto>> GetByTouristId(long touristId)
    {
       /* var result = _touristPositionService.GetByTouristId(touristId);
        return CreateResponse(result);*/
        var client = _factory.CreateClient();
        var position = await GetTouristPositionGo(client, touristId);
        return position;
    }

    static async Task<TouristPositionResponseDto> AddPositionGo(HttpClient httpClient, TouristPositionCreateDto position)
    {
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(position),
            Encoding.UTF8,
            "application/json");
        Console.WriteLine(jsonContent);

        using HttpResponseMessage response = await httpClient.PostAsync(
            "http://localhost:8081/touristposition",
            jsonContent);;
        var positionResponse = await response.Content.ReadFromJsonAsync<TouristPositionResponseDto>();
        return positionResponse;
    }

    static async Task<TouristPositionResponseDto> GetTouristPositionGo(HttpClient httpClient, long touristId)
    {
        var position = await httpClient.GetFromJsonAsync<TouristPositionResponseDto>(
            "http://localhost:8081/touristposition/" + touristId);
        return position;

    }

    static async Task<TouristPositionResponseDto> UpdatePositionGo(HttpClient httpClient, TouristPositionUpdateDto position)
    {
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(position),
            Encoding.UTF8,
            "application/json");
        Console.WriteLine(jsonContent);

        using HttpResponseMessage response = await httpClient.PutAsync(
            "http://localhost:8081/touristposition",
            jsonContent);

        var updatedPosition = await response.Content.ReadFromJsonAsync<TouristPositionResponseDto>();
        return updatedPosition;
    }

}
