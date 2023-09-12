﻿using CDI.CovidApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CDI.CovidApp.MVC.Controllers
{
    public class CovidDataController : Controller
    {
        private readonly ICovidDataService _covidDataService;

        public CovidDataController(ICovidDataService covidDataService)
        {
            _covidDataService = covidDataService;
        }
        [HttpGet]
        [Route("")]
        [Route("covid-data")]
        public async Task<IActionResult> Index([FromQuery] string? country = null)
        {
            var covidData = await _covidDataService.GetTotalCovidData(country);
            ViewBag.Country = country;

            return Ok(covidData);
        }
    }
}