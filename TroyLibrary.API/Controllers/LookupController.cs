using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TroyLibrary.Common.Models.Lookup;
using TroyLibrary.Service.Interfaces;

namespace TroyLibrary.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [HttpGet]
        public LookupResponse Lookup([FromQuery] string name)
        {
            return new LookupResponse
            {
                Items = _lookupService.Lookup(name)
            };
        }
    }
}
