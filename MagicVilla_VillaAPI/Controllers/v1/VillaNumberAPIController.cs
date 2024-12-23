using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaNumberRepository _villaNumDb;
        private readonly IVillaRepository _villaDb;
        private readonly IMapper _mapper;

        public VillaNumberAPIController(IVillaNumberRepository villaNumDb, IVillaRepository villaDb, IMapper mapper)
        {
            _villaNumDb = villaNumDb;
            _response = new();
            _mapper = mapper;
            _villaDb = villaDb;
        }

        [HttpGet("{villaNo:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villaNum = await _villaNumDb.GetAsync(v => v.VillaNo == villaNo);

                if (villaNum == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaNumberDto>(villaNum);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return _response;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                var villaNumberList = await _villaNumDb.GetAllAsync(includeProperties: "Villa");

                _response.Result = _mapper.Map<List<VillaNumberDto>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] CreateVillaNumberDto entity)
        {
            try
            {
                if (entity == null || entity.VillaNo == 0)
                {
                    return BadRequest();
                }

                var villaNum = await _villaNumDb.GetAsync(v => v.VillaNo == entity.VillaNo, isTracked: false);

                if (villaNum != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Number Already Exists!");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaDb.GetAsync(v => v.Id == entity.VillaId, isTracked: false);

                if (villa == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Id is Invalid");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var model = _mapper.Map<VillaNumber>(entity);
                model.CreatedDate = DateTime.Now;

                await _villaNumDb.CreateAsync(model);

                _response.Result = _mapper.Map<VillaNumberDto>(model);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVillaNumber", new { villaNo = model.VillaNo }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }

            return _response;
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{villaNo:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int villaNo, UpdateVillaNumberDto updateDto)
        {
            try
            {
                if (villaNo != updateDto.VillaNo)
                {
                    return BadRequest();
                }

                var villaNumIsFound = await _villaNumDb.GetAsync(v => v.VillaNo == villaNo, isTracked: false);

                if (villaNumIsFound == null)
                {
                    return BadRequest();
                }

                var villa = await _villaDb.GetAsync(v => v.Id == updateDto.VillaId, isTracked: false);

                if (villa == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is invalid");
                    return BadRequest();
                }


                var villaNum = _mapper.Map<VillaNumber>(updateDto);

                villaNum.CreatedDate = villaNumIsFound.CreatedDate;

                var updatedVillaNum = await _villaNumDb.UpdateAsync(villaNum);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaNumberDto>(updatedVillaNum);

                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }

            return _response;
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{villaNo:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int villaNo)
        {
            try
            {
                var villaNumIsFound = await _villaNumDb.GetAsync(v => v.VillaNo == villaNo);

                if (villaNumIsFound == null)
                {
                    return NotFound();
                }

                await _villaNumDb.RemoveAsync(villaNumIsFound);

                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }

            return _response;
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{villaNo:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePartialVillaNumber(int villaNo, JsonPatchDocument<UpdateVillaNumberDto> patchDto)
        {
            if (villaNo == 0 || patchDto == null)
            {
                return BadRequest();
            }

            var villaNum = await _villaNumDb.GetAsync(v => v.VillaNo == villaNo, isTracked: false);

            if (villaNum == null)
            {
                return BadRequest();
            }

            UpdateVillaNumberDto updateDto = _mapper.Map<UpdateVillaNumberDto>(villaNum);

            patchDto.ApplyTo(updateDto, ModelState);


            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            VillaNumber updatedVillaNumber = _mapper.Map<VillaNumber>(updateDto);

            updatedVillaNumber.CreatedDate = villaNum.CreatedDate;

            await _villaNumDb.UpdateAsync(updatedVillaNumber);

            return NoContent();
        }

    }
}
