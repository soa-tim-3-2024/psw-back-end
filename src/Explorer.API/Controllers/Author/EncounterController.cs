using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Author;

[Authorize(Policy = "authorPolicy")]
[Route("api/author/encounter")]
public class EncounterController : BaseApiController
{
    private readonly IEncounterService _encounterService;
    private readonly IHttpClientFactory _factory;
    public EncounterController(IEncounterService encounterService, IHttpClientFactory factory)
    {
        _encounterService = encounterService;
        _factory = factory;
    }

    [HttpPost]


    public async Task<ActionResult<KeyPointEncounterCreateDto>> CreateKeyPointEncounter([FromBody] KeyPointEncounterCreateDto keyPointEncounter)
    {
        var client = _factory.CreateClient();
        long userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
        var encounterResponse = await CreateKeyPointEncounterGo(client, keyPointEncounter);
        return encounterResponse;
    }

    static async Task<KeyPointEncounterCreateDto> CreateKeyPointEncounterGo(HttpClient httpClient, KeyPointEncounterCreateDto encounter)
    {
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(encounter),
            Encoding.UTF8,
            "application/json");
        Console.WriteLine(jsonContent);

        using HttpResponseMessage response = await httpClient.PostAsync(
            "http://host.docker.internal:8082/keyPoint/encounters",
            jsonContent);
        Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
        var encounterResponse = await response.Content.ReadFromJsonAsync<KeyPointEncounterCreateDto>();
        return encounterResponse;
    }
}

