using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace movie_rental_system.api.Controllers
{
    [Route("api")]
    [ApiController]
    public class IntroController : ControllerBase
    {
        [HttpGet]
        public APIMeta Get()
        {
            return new APIMeta
            {
                API = "movie-rental-api",
                Version = "1.0.0"
            };
        }
    }

    public struct APIMeta
    {
        public string API { get; set; }
        public string Version { get; set; }
    }
}