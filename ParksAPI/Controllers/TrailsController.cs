using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParksAPI.Models;
using ParksAPI.Models.DTOs;
using ParksAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParksAPI.Controllers
{
    [Route("api/Trails")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetTails()
        {
            var list = _trailRepo.GetTrails();
            var objDto = new List<TrailDto>();
            
            foreach (var item in list)
            {
                objDto.Add(_mapper.Map<TrailDto>(item));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Get trail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}", Name = "GetTrial")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int id)
        {
            var obj = _trailRepo.GetTrail(id);

            if (obj == null)
            {
                return NotFound("Trail does not exist");
            }
            var objDto = _mapper.Map<TrailDto>(obj);
           
            return Ok(objDto);
        }

        /// <summary>
        /// Create trail
        /// </summary>
        /// <param name="trailDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists");
                return StatusCode(404, ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record { trailObj.Name } ");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { id = trailObj.Id }, trailObj);
        }

        /// <summary>
        /// Update trail
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trailDto"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{id}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]           
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateTrail(int id, [FromBody] TrailUpdateDto trailDto)
        {

            if (trailDto == null || id != trailDto.Id)
            {
                return BadRequest(ModelState);
            }

            var trailDtoObj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepo.UpdateTrail(trailDtoObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record { trailDtoObj.Name } ");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete trail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int id)
        {

            if (!_trailRepo.TrailExists(id))
            {
                return NotFound();
            }

            var obj = _trailRepo.GetTrail(id);

            if (!_trailRepo.DeleteTrail(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record { obj.Name } ");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
