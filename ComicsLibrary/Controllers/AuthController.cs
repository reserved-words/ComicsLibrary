using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ComicsLibrary.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Settings()
        {
            var authSection = _config.GetSection("Auth");

            return new JsonResult(new {
                Url = authSection["Url"],
                ClientId = authSection["ClientID"],
                ResponseType = authSection["ResponseType"],
                Scope = authSection["Scope"]
            });
        }
    }
}