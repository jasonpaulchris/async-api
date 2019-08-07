using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookCovers.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCoversController : ControllerBase
    {
        [HttpGet("{name}")]
        public async Task<IActionResult> GetBookCover(string name, bool returnFault = false)
        {
            if (returnFault)
            {
                await Task.Delay(500);
                return new StatusCodeResult(500);
            }

            var random = new Random();
            int fakeCoverBytes = random.Next(2097152, 10485760);
            byte[] fakeCover = new byte[fakeCoverBytes];
            random.NextBytes(fakeCover);

            return Ok(new
            {
                Name = name,
                Content = fakeCover
            });
        }
    }
}