using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Core.UseCases.TourAuthoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text;

namespace Explorer.API.Controllers.Author.TourAuthoring;

[Route("api/tour-authoring")]
public class KeyPointController : BaseApiController
{
    private readonly IKeyPointService _keyPointService;
    private static readonly HttpClient _sharedClient = new();

    public KeyPointController(IKeyPointService keyPointService)
    {
        _keyPointService = keyPointService;
    }

    [Authorize(Roles = "author")]
    [HttpPost("tours/{tourId:long}/key-points")]
    public async Task<ActionResult<KeyPointResponseDto>> CreateKeyPoint([FromRoute] long tourId, [FromBody] KeyPointCreateDto keyPoint)
    {
        keyPoint.TourId = tourId;
        //var result = _keyPointService.Create(keyPoint);
        //return CreateResponse(result);
        var result = await CreateKeyPointGo(_sharedClient, keyPoint);
        return result;
    }
    static async Task<KeyPointResponseDto> CreateKeyPointGo(HttpClient httpClient, KeyPointCreateDto kp)
    {
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(kp),
            Encoding.UTF8,
            "application/json");
        Console.WriteLine(jsonContent);

        using HttpResponseMessage response = await httpClient.PostAsync(
            "http://localhost:8081/keyPoints",
            jsonContent);
        Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
        var kpResponse = await response.Content.ReadFromJsonAsync<KeyPointResponseDto>();
        return kpResponse;
    }
    [Authorize(Roles = "author")]
    [HttpPut("tours/{tourId:long}/key-points/{id:long}")]
    public ActionResult<KeyPointResponseDto> Update(long tourId, long id, [FromBody] KeyPointUpdateDto keyPoint)
    {
        keyPoint.Id = id;
        var result = _keyPointService.Update(keyPoint);
        return CreateResponse(result);
    }

    [Authorize(Roles = "author, tourist")]
    [HttpDelete("tours/{tourId:long}/key-points/{id:long}")]
    public ActionResult Delete(long tourId, long id)
    {
        var result = _keyPointService.Delete(id);
        return CreateResponse(result);
    }

    [Authorize(Roles = "author")]
    [HttpGet]
    public ActionResult<PagedResult<KeyPointResponseDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _keyPointService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }
}
