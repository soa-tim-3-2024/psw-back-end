using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using System.Diagnostics;
using FluentResults;
using Explorer.Tours.Core.Domain.Tours;
using System.Net.Http;

namespace Explorer.API.Controllers.Author.TourAuthoring
{

    [Route("api/tour")]
    public class TourController : BaseApiController
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        private static readonly HttpClient _sharedClient = new();

        [Authorize(Roles = "author")]
        [HttpGet]
        public ActionResult<PagedResult<TourResponseDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetAllPaged(page, pageSize);
            return CreateResponse(result);
        }

        [Authorize(Roles = "author")]
        [HttpGet("published")]
        public ActionResult<PagedResult<TourResponseDto>> GetPublished([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetPublished(page, pageSize);
            return CreateResponse(result);
        }

        [Authorize(Roles = "author, tourist")]
        [HttpGet("authors")]
        public async Task<ActionResult<List<TourResponseDto>>> GetAuthorsTours([FromQuery] int page, [FromQuery] int pageSize)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var id = long.Parse(identity.FindFirst("id").Value);
            //var result = _tourService.GetAuthorsPagedTours(id, page, pageSize);
            //return CreateResponse(result);*/
            //var tours = await GetAuthorsToursGo(_sharedClient, id);
            //return tours;
            var httpResponse = await _sharedClient.GetAsync("http://host.docker.internal:8083/tours/" + id);

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<TourResponseDto[]>();
                return Ok(new List<TourResponseDto>(response.ToList()));
            }
            else
            {
                var resp = new ContentResult
                {
                    StatusCode = (int)httpResponse.StatusCode,
                    Content = await httpResponse.Content.ReadAsStringAsync(),
                    ContentType = "text/plain"
                };
                return resp;
            }
        }
/*
        [Authorize(Roles = "author, tourist")]
        [HttpPost]
        public async Task<ActionResult<TourResponseDto>> Create([FromBody] TourCreateDto tour)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                tour.AuthorId = long.Parse(identity.FindFirst("id").Value);
            }
            /*var result = _tourService.Create(tour);
            return CreateResponse(result);
            var tourResponse = await CreateTourGo(_sharedClient, tour);
            return tourResponse;
        }
*/
    /*    [Authorize(Roles = "author, tourist")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TourResponseDto>> Update([FromBody] TourUpdateDto tour)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                tour.AuthorId = long.Parse(identity.FindFirst("id").Value);
            }
            /*var result = _tourService.Update(tour);
            return CreateResponse(result);
            
            var tourResponse = await UpdateTourGo(_sharedClient, tour);
            return tourResponse;
        }
    */
        [Authorize(Roles = "author, tourist")]
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _tourService.DeleteCascade(id);
            return CreateResponse(result);
        }

        [Authorize(Roles = "author, tourist")]
        [HttpGet("equipment/{tourId:int}")]
        public async Task<ActionResult<List<EquipmentResponseDto>>> GetEquipment(int tourId)
        {
            //var result = _tourService.GetEquipment(tourId);
            //return CreateResponse(result);
            var eq = await _sharedClient.GetFromJsonAsync<List<EquipmentResponseDto>>(
                "http://host.docker.internal:8083/equipment/tour/" + tourId);
            return eq;
        }

        [Authorize(Roles = "author, tourist")]
        [HttpPost("equipment/{tourId:int}/{equipmentId:int}")]
        public async Task<ActionResult> AddEquipment(int tourId, int equipmentId)
        {
            //var result = _tourService.AddEquipment(tourId, equipmentId);
            //return CreateResponse(result);
            var eq = await _sharedClient.PostAsync(
                "http://host.docker.internal:8083/equipment/" + equipmentId +"/"+ tourId, null);
            if(eq != null)
            {
                return Ok(eq);

            }
            return NotFound(eq);
        }

        [Authorize(Roles = "author, tourist")]
        [HttpDelete("equipment/{tourId:int}/{equipmentId:int}")]
        public async Task<ActionResult> DeleteEquipment(int tourId, int equipmentId)
        {
            //var result = _tourService.DeleteEquipment(tourId, equipmentId);
            //return CreateResponse(result);
            var eq = await _sharedClient.DeleteAsync(
                "http://host.docker.internal:8083/equipment/" + equipmentId + "/" + tourId);
            if (eq != null)
            {
                return Ok(eq);

            }
            return NotFound(eq);
        }

        [Authorize(Roles = "author, tourist")]
        [HttpGet("{tourId:long}")]
        public ActionResult<PagedResult<TourResponseDto>> GetById(long tourId)
        {
            var result = _tourService.GetById(tourId);
            return CreateResponse(result);
        }

        [Authorize(Roles = "author")]
        [HttpPut("publish/{id:int}")]
        public async Task<ActionResult<int>> Publish(long id, [FromBody]TourUpdateDto tour)
        {
            var tourResponse = await PublishTourGo(_sharedClient, tour);
            return tourResponse;
        }

        [Authorize(Roles = "author")]
        [HttpPut("archive/{id:int}")]
        public async Task<ActionResult<int>> Archive(long id, [FromBody]TourUpdateDto tour)
        {
            var tourResponse = await ArchiveTourGo(_sharedClient, tour);
            return tourResponse;
        }

        [Authorize(Roles = "tourist")]
        [HttpPut("markAsReady/{id:int}")]
        public ActionResult<TourResponseDto> MarkAsReady(long id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            long authorId = -1;
            if (identity != null && identity.IsAuthenticated)
            {
                authorId = long.Parse(identity.FindFirst("id").Value);
            }
            var result = _tourService.MarkAsReady(id, authorId);
            return CreateResponse(result);
        }

        [Authorize(Roles = "tourist")]
        [HttpGet("recommended/{publicKeyPointIds}")]
        public ActionResult<TourResponseDto> GetRecommended([FromQuery] int page, [FromQuery] int pageSize, string publicKeyPointIds)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            long authorId = -1;
            if (identity != null && identity.IsAuthenticated)
            {
                authorId = long.Parse(identity.FindFirst("id").Value);
            }

            var keyValuePairs = publicKeyPointIds.Split('=');

            var keyPointIdsList = keyValuePairs[1].Split(',').Select(long.Parse).ToList();

            var result = _tourService.GetToursBasedOnSelectedKeyPoints(page, pageSize, keyPointIdsList, authorId);
            return CreateResponse(result);
        }
        static async Task<TourResponseDto> CreateTourGo(HttpClient httpClient, TourCreateDto tour)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(tour),
                Encoding.UTF8,
                "application/json");
            Console.WriteLine(jsonContent);

            using HttpResponseMessage response = await httpClient.PostAsync(
                "http://host.docker.internal:8083/tours",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            var tourResponse = await response.Content.ReadFromJsonAsync<TourResponseDto>();
            return tourResponse;
        }
        static async Task<List<TourResponseDto>> GetAuthorsToursGo(HttpClient httpClient, long authorId)
        {
            var tours = await httpClient.GetFromJsonAsync<TourResponseDto[]>(
                "http://host.docker.internal:8083/tours/" + authorId);
            return tours.ToList();
        }
        static async Task<TourResponseDto> UpdateTourGo(HttpClient httpClient, TourUpdateDto tour)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(tour),
                Encoding.UTF8,
                "application/json");
            Console.WriteLine(jsonContent);

            using HttpResponseMessage response = await httpClient.PutAsync(
                "http://host.docker.internal:8083/tours",
                jsonContent);

            var updatedTour = await response.Content.ReadFromJsonAsync<TourResponseDto>();
            return updatedTour;
        }
        static async Task<int> PublishTourGo(HttpClient httpClient, TourUpdateDto tour)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(tour),
                Encoding.UTF8,
                "application/json");
            Console.WriteLine(jsonContent);

            using HttpResponseMessage response = await httpClient.PutAsync(
                "http://host.docker.internal:8083/tours/publish",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            return 1;
        }

        static async Task<int> ArchiveTourGo(HttpClient httpClient, TourUpdateDto tour)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(tour),
                Encoding.UTF8,
                "application/json");
            Console.WriteLine(jsonContent);

            using HttpResponseMessage response = await httpClient.PutAsync(
                "http://host.docker.internal:8083/tours/archive",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            return 1;
        }

    }


}
