using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParksAPI.Models;
using ParksAPI.Models.DTOs;
using ParksAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepository;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepository = npRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var list = _npRepository.GetNationalParks();
            var objDto = new List<NationalParkDto>();
            
            foreach (var item in list)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(item));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Get national park by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int id)
        {
            var obj = _npRepository.GetNationalPark(id);

            if (obj == null)
            {
                return NotFound("Nation Park does not exist");
            }
            var objDto = _mapper.Map<NationalParkDto>(obj);
           
            return Ok(objDto);
        }

        /// <summary>
        /// Create national park
        /// </summary>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_npRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists");
                return StatusCode(404, ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepository.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record { nationalParkObj.Name } ");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { id = nationalParkObj.Id }, nationalParkObj);
        }

        /// <summary>
        /// Update national park
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{id}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int id, [FromBody] NationalParkDto nationalParkDto)
        {

            if (nationalParkDto == null || id != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepository.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record { nationalParkObj.Name } ");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete national park
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}", Name = "DeleteNationalPark")]
        public IActionResult DeleteNationalPark(int id)
        {

            if (!_npRepository.NationalParkExists(id))
            {
                return NotFound();
            }

            var obj = _npRepository.GetNationalPark(id);

            if (!_npRepository.DeleteNationalPark(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record { obj.Name } ");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
