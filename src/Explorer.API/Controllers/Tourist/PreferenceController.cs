using Explorer.Tours.API.Public;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Explorer.Tours.Core.Domain.Tours;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using System.Net.Http;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/preferences")]
    public class PreferenceController : BaseApiController
    {
        private readonly IPreferenceService _tourPreferencesService;
        private static readonly HttpClient _sharedClient = new();

        public PreferenceController(IPreferenceService tourPreferencesService)
        {
            _tourPreferencesService = tourPreferencesService;
        }

        [HttpGet]
        public async Task<ActionResult<PreferenceResponseDto>> Get()
        {
           // int userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            int id = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                id = int.Parse(identity.FindFirst("id").Value);
            }
            //var result = _tourPreferencesService.GetByUserId(id);
            //return CreateResponse(result);
            var pref = await GetPrefGo(_sharedClient, id);
            return pref;
        }
        static async Task<PreferenceResponseDto> GetPrefGo(HttpClient httpClient, long id)
        {
            var pref = await httpClient.GetFromJsonAsync<PreferenceResponseDto>(
                "http://localhost:8081/preference/" + id);
            return pref;

        }

        [HttpPost("create")]
        public async Task<ActionResult<PreferenceResponseDto>> Create([FromBody] PreferenceCreateDto preference)
        {
            preference.UserId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            //var result = _tourPreferencesService.Create(preference);
            //return CreateResponse(result);
            var tourResponse = await CreatePreferenceGo(_sharedClient, preference);
            return tourResponse;
        }

        static async Task<PreferenceResponseDto> CreatePreferenceGo(HttpClient httpClient, PreferenceCreateDto pref)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(pref),
                Encoding.UTF8,
                "application/json");
            Console.WriteLine(jsonContent);

            using HttpResponseMessage response = await httpClient.PostAsync(
                "http://localhost:8081/preference",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            var prefResponse = await response.Content.ReadFromJsonAsync<PreferenceResponseDto>();
            return prefResponse;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            //var result = _tourPreferencesService.Delete(id);
            //return CreateResponse(result);
            var response = await _sharedClient.DeleteAsync(
                "http://localhost:8081/preference/" + id);
            return Ok(response.Content);
        }

        [HttpPut]
        public async Task<ActionResult<PreferenceResponseDto>> Update([FromBody] PreferenceUpdateDto preference)
        {
            preference.UserId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            //var result = _tourPreferencesService.Update(preference);
            //return CreateResponse(result);
            var tourResponse = await UpdatePreferenceGo(_sharedClient, preference);
            return tourResponse;
        }

        static async Task<PreferenceResponseDto> UpdatePreferenceGo(HttpClient httpClient, PreferenceUpdateDto pref)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(pref),
                Encoding.UTF8,
                "application/json");
            Console.WriteLine(jsonContent);

            using HttpResponseMessage response = await httpClient.PutAsync(
                "http://localhost:8081/preference",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            var prefResponse = await response.Content.ReadFromJsonAsync<PreferenceResponseDto>();
            return prefResponse;
        }
    }
}
