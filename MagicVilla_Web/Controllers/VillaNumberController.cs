using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

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

            }

            List<SelectListItem> selectList = new();


            foreach (VillaDto villa in villas)
            {
                selectList.Add(new SelectListItem { Text = villa.Name, Value = villa.Id.ToString() });
            }

            ViewBag.Villas = selectList;

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
            }

            TempData["error"] = "Error Encountered";
            return View(villaNumber);
        }
        
    }
}
