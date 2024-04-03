using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.TouristPosition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Security.Claims;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/encounter")]
    public class EncounterController : BaseApiController
    {
        private readonly IEncounterService _encounterService;
        private readonly ITouristProgressService _progressService;
        private readonly IUserService _userService;
        private readonly IHttpClientFactory _factory;
        public EncounterController(IEncounterService encounterService, ITouristProgressService progressService, IUserService userService, IHttpClientFactory factory)
        {
            _encounterService = encounterService;
            _progressService = progressService;
            _userService = userService;
            _factory = factory; _factory = factory;
        }

        [HttpGet("{encounterId:long}/instance")]
        public async Task<ActionResult<bool>> GetInstance(int encounterId)
        {
            //long userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            //var result = _encounterService.GetInstance(userId, encounterId);
            //return CreateResponse(result);
            var client = _factory.CreateClient();
            int userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            var result = await GetInstanceGo(client, userId, encounterId);
            return Ok(result);
        }

        [HttpPost("{id}/activate")]
        public async Task<ActionResult<EncounterResponseDto>> Activate([FromBody] TouristPositionCreateDto position, string id)
        {
            var client = _factory.CreateClient();
            long userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            position.TouristId = userId;

            var result = await ActivateGo(client, position, id);
            if(result.Value != null && result.Value.Id == -1)
            {
                return BadRequest();
            }

            //var result = _encounterService.ActivateEncounter(userId, id, position.Longitude, position.Latitude);
            return Ok(result);
        }

        [HttpPost("{id:long}/complete")]
        public async Task<ActionResult<EncounterResponseDto>> Complete(int id)
        {
            //long userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            //var result = _encounterService.CompleteEncounter(userId, id);
            //return CreateResponse(result);
            var client = _factory.CreateClient();
            int userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            var result = await CompleteGo(client, userId, id);
            if(result.Value != null && result.Value.Id == -1){
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet("{id:long}/cancel")]
        public async Task<ActionResult<EncounterResponseDto>> Cancel(int id)
        {
            //long userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            //var result = _encounterService.CancelEncounter(userId, id);
            //return CreateResponse(result);
            var client = _factory.CreateClient();
            int userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            var result = await CancelGo(client, userId, id);
            if(result.Value != null && result.Value.Id == -1){
                return BadRequest();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("{id:long}")]
        public ActionResult<EncounterResponseDto> Get(long id)
        {
            var result = _encounterService.Get(id);
            return CreateResponse(result);
        }

        [HttpGet]
        public ActionResult<PagedResult<EncounterResponseDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _encounterService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost("in-range-of")]
        /*public ActionResult<PagedResult<EncounterResponseDto>> GetAllInRangeOf([FromBody] UserPositionWithRangeDto position, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _encounterService.GetAllInRangeOf(position.Range, position.Longitude, position.Latitude, page, pageSize);
            return CreateResponse(result);
        } */

        public async Task<ActionResult<List<EncounterResponseDto>>> GetAllInRangeOf([FromBody] UserPositionWithRangeDto position, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var client = _factory.CreateClient();
            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //var id = long.Parse(identity.FindFirst("id").Value);
            //var tours = await GetAllEncountersGo(client);
            //return tours;
            var httpResponse = await client.GetAsync("http://host.docker.internal:8082/encounters/all/")

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<EncounterResponseDto[]>();
                return Ok(new List<EncounterResponseDto>(response.ToList()));
            }
            else
            {
                return new ContentResult
                {
                    StatusCode = (int)httpResponse.StatusCode,
                    Content = await httpResponse.Content.ReadAsStringAsync(),
                    ContentType = "text/plain"
                };
            }
            
        }



        [HttpGet("done-encounters")]
        public async Task<ActionResult<List<EncounterResponseDto>>> GetAllDoneByUser(int currentUserId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            //var result = _encounterService.GetAllDoneByUser(currentUserId, page, pageSize);
            //return CreateResponse(result);
            var client = _factory.CreateClient();
            int userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            var result = await GetAllDoneByUserGo(client, userId);
            return result;
        }

        [HttpGet("active")]
        public ActionResult<PagedResult<EncounterResponseDto>> GetActive([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _encounterService.GetActive(page, pageSize);
            return CreateResponse(result);
        }


        [HttpPost("key-point/{keyPointId:long}")]
        public ActionResult<KeyPointEncounterResponseDto> ActivateKeyPointEncounter(
            [FromBody] TouristPositionCreateDto position, long keyPointId)
        {
            long userId = int.Parse(HttpContext.User.Claims
                .First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            var result =
                _encounterService.ActivateKeyPointEncounter(position.Longitude, position.Latitude, keyPointId, userId);

            return CreateResponse(result);
        }

        [HttpGet("progress")]
        /*public ActionResult<TouristProgressResponseDto> GetProgress()
        {
            long userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            var result = _progressService.GetByUserId(userId);

            return CreateResponse(result);
        } */


        public async Task<ActionResult<TouristProgressResponseDto>> GetProgress()
        {
            var client = _factory.CreateClient();
            long userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            var user = _userService.Get(userId).Value;
            var progress = await GetTouristProgressGo(client, userId);
            var result = new TouristProgressResponseDto();
            result.Xp = progress.Xp;
            result.UserId = progress.UserId;
            result.Level = progress.Level;
            result.User = user;
            Debug.WriteLine("test");
            return result;
        }


        static async Task<GolangProgressDto> GetTouristProgressGo(HttpClient httpClient, long userId)
        {
            var progress = await httpClient.GetFromJsonAsync<GolangProgressDto>(
                "http://host.docker.internal:8082/progress/" + userId);
            Debug.WriteLine("test");
            return progress;

        }

        static async Task<ActionResult<EncounterResponseDto>> ActivateGo(HttpClient httpClient, TouristPositionCreateDto position, string id)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(position),
                Encoding.UTF8,
                "application/json");
            Console.WriteLine(jsonContent);
            using HttpResponseMessage response = await httpClient.PostAsync(
                "http://host.docker.internal:8082/encounters/activate/" + id,
                jsonContent);
            if(response.IsSuccessStatusCode)
            {
                Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
                var encounterResponse = await response.Content.ReadFromJsonAsync<EncounterResponseDto>();
                return encounterResponse;
            }
            var temp = new EncounterResponseDto();
            temp.Id = -1;
            return temp;
        }

        static async Task<ActionResult<EncounterResponseDto>> CancelGo(HttpClient httpClient, int userId, int encounterId)
        {
            using HttpResponseMessage response = await httpClient.GetAsync(
               "http://host.docker.internal:8082/encounters/cancel/" + userId + "/" + encounterId);
            var encounterResponse = await response.Content.ReadFromJsonAsync<EncounterResponseDto>();
            if(response.IsSuccessStatusCode)
            {
                var temp = new EncounterResponseDto();
                temp.Id = encounterId;
                return temp;
            }
            else
            {
                var temp = new EncounterResponseDto();
                temp.Id = -1;
                return temp;
            }
        }

        static async Task<ActionResult<EncounterResponseDto>> CompleteGo(HttpClient httpClient, int userId, int encounterId)
        {
            using HttpResponseMessage response = await httpClient.GetAsync(
               "http://host.docker.internal:8082/encounters/complete/" + userId + "/" + encounterId);
            if (response.IsSuccessStatusCode)
            {
                var encounterResponse = await response.Content.ReadFromJsonAsync<EncounterResponseDto>();
                var temp = new EncounterResponseDto();
                temp.Id = encounterId;
                return temp;
            }
            else
            {
                var temp = new EncounterResponseDto();
                temp.Id = -1;
                return temp;
            }
        }
        
        static async Task<List<EncounterResponseDto>> GetAllEncountersGo(HttpClient httpClient)
        {
            var encounters = await httpClient.GetFromJsonAsync<List<EncounterResponseDto>>(
                "http://host.docker.internal:8082/encounters/all/");
            return encounters;

        }

        static async Task<List<EncounterResponseDto>> GetAllDoneByUserGo(HttpClient httpClient, int currentUserId)
        {
            var encounters = await httpClient.GetFromJsonAsync<List<EncounterResponseDto>>(
                "http://host.docker.internal:8082/encounters/getCompletedByUser/" + currentUserId);
            return encounters;
        }

        static async Task<bool> GetInstanceGo(HttpClient httpClient, int userId, int encounterId)
        {
            var isCompleted = await httpClient.GetFromJsonAsync<bool>(
                "http://host.docker.internal:8082/encounters/comleted/" + userId + "/" + encounterId);
            return isCompleted;
        }


    }
}
