using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService, IMapper mapper)
        {
            _villaNumberService = villaNumberService;
            _villaService = villaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDto> list = new();

            var response = await _villaNumberService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<IActionResult> CreateVillaNumber()
        {
            List<VillaDto> villas = new();

            var response = await _villaService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSuccess)
            {
                villas = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
                List<SelectListItem> selectList = new();


                foreach (VillaDto villa in villas)
                {
                    selectList.Add(new SelectListItem { Text = villa.Name, Value = villa.Id.ToString() });
                }

                ViewBag.Villas = selectList;
                
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVillaNumber(CreateVillaNumberDto villaNumber)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(villaNumber);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Number Created Successfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            TempData["error"] = "Error Encountered";
            return View(villaNumber);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            var villaNumberRes = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            List<SelectListItem> selectList = new();

            if (villaNumberRes != null && villaNumberRes.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<UpdateVillaNumberDto>(Convert.ToString(villaNumberRes.Result));

                var villasRes = await _villaService.GetAllAsync<APIResponse>();
                var villas = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(villasRes.Result));



                foreach(var villa in villas)
                {
                    selectList.Add(new SelectListItem()
                    {
                        Text = villa.Name,
                        Value = villa.Id.ToString(),
                    });
                }

                ViewBag.Villas = selectList;

                return View(model);
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateVillaNumber(UpdateVillaNumberDto updateVillaDto)
        {
            if (updateVillaDto == null)
            {
                return BadRequest();
            }

            var response = await _villaNumberService.UpdateAsync<APIResponse>(updateVillaDto);
            
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Number updated successfully";
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            else
            {
                if (response.ErrorMessages.Count > 1)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }

            TempData["error"] = "Error Encountered";
            return View(updateVillaDto);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            VillaNumberDto villaNumber = new();

            if (response != null && response.IsSuccess)
            {
                villaNumber = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
            }

            response = await _villaService.GetAllAsync<APIResponse>();
            List<VillaDto> villaList = new();
            IEnumerable<SelectListItem> selectList;

            if (response != null && response.IsSuccess)
            {
                villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));

                selectList = villaList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

                ViewBag.Villas = selectList;
                return View(villaNumber);
            }
            
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDto villaNumber)
        {
            var response = await _villaNumberService.DeleteAsync<APIResponse>(villaNumber.VillaNo);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            return View(villaNumber);
        }

    }
}
