using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Microsoft.Extensions.Options;
namespace RedisExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private const string CacheKey = "CurrentTime";

        public ValuesController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var currentTimeBytes = await _cache.GetAsync(CacheKey);

            if (currentTimeBytes == null)
            {
                currentTimeBytes = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());                

                var cacheOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.MaxValue);
                await _cache.SetAsync(CacheKey, currentTimeBytes, cacheOptions);
            }

            var currentTimeBinary = BitConverter.ToInt64(currentTimeBytes);

            var currentTime = DateTime.FromBinary(currentTimeBinary);
            return currentTime.ToString();
        }

        [HttpGet("reset")]
        public async Task Reset()
        {
            await _cache.RemoveAsync(CacheKey);
        }
    }
}
