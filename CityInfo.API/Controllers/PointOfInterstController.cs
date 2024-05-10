using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [Authorize]
    [ApiController]
    public class PointOfInterstController : ControllerBase
    {

       private readonly ILogger<PointOfInterstController> _logger;
       private readonly IMailService _mailService;
       private readonly ICityInfoRepository _cityInfoRepository;
       private readonly IMapper _mapper;

        public PointOfInterstController (ILogger<PointOfInterstController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper

            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterstsDto>>> getPointsOfInterestAsync (int cityId)
        { 
           if(!await _cityInfoRepository.CityExistsAysnc(cityId))
            {
                return NotFound();
            }
            var pointsOfInterest = await _cityInfoRepository.GetPointOfInterests(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterstsDto>>(pointsOfInterest));
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterstsDto>> getPointOfInterest(int cityId, int pointofinterestid)
        {
            if(!await _cityInfoRepository.CityExistsAysnc(cityId))
            {
                return NotFound();
            }
            var pointOfInterest = await _cityInfoRepository.GetPointOfInterest(cityId, pointofinterestid);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PointOfInterstsDto>(pointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterstsDto>> CreatePointOfInterest(
            int cityId,
            CreatePointOfInterestDto pointofinterest)
        {
            if (!await _cityInfoRepository.CityExistsAysnc(cityId))
            {
                return NotFound();
            }

            var finalpointofinterest = _mapper.Map<PointOfInterest>(pointofinterest);

            await _cityInfoRepository.AddPointOfInterestForCity(cityId, finalpointofinterest);

            await _cityInfoRepository.SaveChangesAysnc();

            var createdPointOfInterestToReturn = _mapper.Map<PointOfInterstsDto>(finalpointofinterest);
            
            return CreatedAtRoute("GetPointOfInterest",
                new { cityId, pointofinterestid = createdPointOfInterestToReturn.Id },
                createdPointOfInterestToReturn);

        }

        [HttpPut("{pointofinterestid}")]
        public async Task<ActionResult> UpdatePointOfInterest(
            int cityId,
            int pointofinterestid,
            UpdatePointOfInterestDto pointOfInterest)
        {

            if (!await _cityInfoRepository.CityExistsAysnc(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterest(cityId, pointofinterestid);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }
            _mapper.Map(pointOfInterest, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAysnc();

            return NoContent();

        }


        [HttpPatch("{pointofinterestid}")]

        public async Task<ActionResult> PartiallyUpdatePointOfInterest(
            int cityId,
            int pointofinterestid,
            JsonPatchDocument<UpdatePointOfInterestDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAysnc(cityId))
            {
                return NotFound();
            }

            var pointOfInTerestEntity = await _cityInfoRepository.GetPointOfInterest(cityId, pointofinterestid);
            if (pointOfInTerestEntity == null)
            {
                return NotFound();
            }
            var pointOfInterestToPatch = _mapper.Map<UpdatePointOfInterestDto>(pointOfInTerestEntity);
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(pointOfInterestToPatch, pointOfInTerestEntity);
            await _cityInfoRepository.SaveChangesAysnc();

            return NoContent();
        }

        [HttpDelete("{pointofinterestid}")]
        public async Task<ActionResult> DeletePointOfInterest(
            int cityId,
            int pointofinterestid
            )
        {
            if (!await _cityInfoRepository.CityExistsAysnc(cityId))
            {
                return NotFound();
            }
            var pointOfInTerestEntity = await _cityInfoRepository.GetPointOfInterest(cityId, pointofinterestid);
            if (pointOfInTerestEntity == null)
            {
                return NotFound();
            }
            _cityInfoRepository.DeletePointOfInterest(pointOfInTerestEntity);
             await _cityInfoRepository.SaveChangesAysnc();

            _mailService.Send("Point of interest deleted.", $"Point of interest {pointOfInTerestEntity.Name} with id {pointofinterestid} was deleted.");
           
            return NoContent();
        }

    }
}