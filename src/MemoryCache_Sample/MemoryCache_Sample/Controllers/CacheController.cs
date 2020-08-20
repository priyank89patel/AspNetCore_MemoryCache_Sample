using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MemoryCache_Sample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCache_Sample.Controllers
{
    [Produces("application/json")]
    [Route("api/cache")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public CacheController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpGet("get/{cacheKey}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCacheContents(string cacheKey)
        {
            object cacheContents = null;
            try
            {
                cacheContents = _cache.Get(cacheKey);

                if (cacheContents == null) return NotFound();
            }
            catch (Exception ex)
            {
                //Log ex
            }

            return Ok(cacheContents);
        }

        [HttpPost("set")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult SetCacheContents(CacheRequest request)
        {
            object cacheContents = null;
            try
            {
                if (request.ExpiresInMinutes < 1 || request.ExpiresInMinutes > 60)
                    return BadRequest("Cache expiry time shoud be between 0 and 60 min(s)");

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(request.ExpiresInMinutes / 2),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(request.ExpiresInMinutes)
                };

                cacheContents = _cache.Set(request.Key, request.Data, cacheEntryOptions);
            }
            catch (Exception ex)
            {
                //Log ex
            }

            return Ok(cacheContents);
        }

        [HttpDelete]
        [Route("remove/{cacheKey}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult RemoveCacheContents(string cacheKey)
        {
            try
            {
                _cache.Remove(cacheKey);
            }
            catch (Exception ex)
            {
                //Log ex
            }

            return Ok();
        }
    }
}
