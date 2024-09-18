using Microsoft.AspNetCore.Mvc;
using WUM.Lib.Interfaces;

namespace WelldoneUserManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService countryService;

        public CountryController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            return Ok(countryService.GetCountries());
        }
    }
}
