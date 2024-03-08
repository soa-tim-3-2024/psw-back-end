using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using System.Diagnostics;

namespace Explorer.API.Controllers.Author.TourAuthoring
{

    [Route("api/tour")]
    public class TourController : BaseApiController
    {
        private readonly ITourService _tourService;
        private readonly IHttpClientFactory _factory;

        public TourController(ITourService tourService, IHttpClientFactory factory)
        {
            _tourService = tourService;
            _factory = factory;
        }

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
            var client = _factory.CreateClient();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var id = long.Parse(identity.FindFirst("id").Value);
            /*var result = _tourService.GetAuthorsPagedTours(id, page, pageSize);
            return CreateResponse(result);*/
            var tours = await GetAuthorsToursGo(client, id);
            return tours;
        }

        [Authorize(Roles = "author, tourist")]
        [HttpPost]
        public async Task<ActionResult<TourResponseDto>> Create([FromBody] TourCreateDto tour)
        {
            var client = _factory.CreateClient();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                tour.AuthorId = long.Parse(identity.FindFirst("id").Value);
            }
            /*var result = _tourService.Create(tour);
            return CreateResponse(result);*/
            var tourResponse = await CreateTourGo(client, tour);
            return tourResponse;
        }

        [Authorize(Roles = "author, tourist")]
        [HttpPut("{id:int}")]
        public ActionResult<TourResponseDto> Update([FromBody] TourUpdateDto tour)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                tour.AuthorId = long.Parse(identity.FindFirst("id").Value);
            }
            var result = _tourService.Update(tour);
            return CreateResponse(result);
        }

        [Authorize(Roles = "author, tourist")]
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _tourService.DeleteCascade(id);
            return CreateResponse(result);
        }

        [Authorize(Roles = "author, tourist")]
        [HttpGet("equipment/{tourId:int}")]
        public ActionResult GetEquipment(int tourId)
        {
            var result = _tourService.GetEquipment(tourId);
            return CreateResponse(result);
        }

        [Authorize(Roles = "author, tourist")]
        [HttpPost("equipment/{tourId:int}/{equipmentId:int}")]
        public ActionResult AddEquipment(int tourId, int equipmentId)
        {
            var result = _tourService.AddEquipment(tourId, equipmentId);
            return CreateResponse(result);
        }

        [Authorize(Roles = "author, tourist")]
        [HttpDelete("equipment/{tourId:int}/{equipmentId:int}")]
        public ActionResult DeleteEquipment(int tourId, int equipmentId)
        {
            var result = _tourService.DeleteEquipment(tourId, equipmentId);
            return CreateResponse(result);
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
        public ActionResult<TourResponseDto> Publish(long id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            long authorId = -1;
            if (identity != null && identity.IsAuthenticated)
            {
                authorId = long.Parse(identity.FindFirst("id").Value);
            }
            var result = _tourService.Publish(id, authorId);
            return CreateResponse(result);
        }

        [Authorize(Roles = "author")]
        [HttpPut("archive/{id:int}")]
        public ActionResult<TourResponseDto> Archive(long id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            long authorId = -1;
            if (identity != null && identity.IsAuthenticated)
            {
                authorId = long.Parse(identity.FindFirst("id").Value);
            }
            var result = _tourService.Archive(id, authorId);
            return CreateResponse(result);
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
                "http://localhost:8081/tours",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            var tourResponse = await response.Content.ReadFromJsonAsync<TourResponseDto>();
            return tourResponse;
        }
        static async Task<List<TourResponseDto>> GetAuthorsToursGo(HttpClient httpClient, long authorId)
        {
            var tours = await httpClient.GetFromJsonAsync<List<TourResponseDto>>(
                "http://localhost:8081/tours/" + authorId);
            return tours;

        }
    }


}
