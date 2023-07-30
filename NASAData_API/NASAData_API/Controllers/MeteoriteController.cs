using Microsoft.AspNetCore.Mvc;
using NASAData_API.Models;
using NASAData_API.DbModels;
using Newtonsoft.Json;
using AutoMapper;
using NASAData_API.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace NASAData_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeteoriteController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IMeteoriteRepository _dbMeteoriteRepository;
        public MeteoriteController(IMeteoriteRepository meteoriteRepository, IMemoryCache memoryCache)
        {
            _dbMeteoriteRepository = meteoriteRepository;
            _memoryCache = memoryCache;
        }
        [HttpGet("GetFilteredList")]
          
        public async Task<IActionResult> GetFilteredList([FromQuery] MeteoriteFilter filter)
        {
            filter.Validate();
            string cashKey = JsonConvert.SerializeObject(filter);

            MeteoriteView[] dbMeteorites = null;
            if (!_memoryCache.TryGetValue(cashKey, out dbMeteorites))
            {
                 dbMeteorites = _dbMeteoriteRepository.GetAllByFilter(filter);

                _memoryCache.Set(cashKey, dbMeteorites, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }
            return Ok(dbMeteorites);
        }

        [HttpGet("GetFilter")]
        public async Task<IActionResult> GetFilter()
        {
            string cashKey = "filterOptions";

            var filter = new { years = new string[] { }, classes = new string[] { } };

            if (!_memoryCache.TryGetValue(cashKey, out filter))
            {
                DbMeteorite[] dbMeteorites = _dbMeteoriteRepository.GetAll();

                 var years = dbMeteorites.GroupBy(m => m.Year).Select(gr => gr.Key.ToString("yyyy")).OrderBy(yer => yer).ToArray();
                 var recclasses = dbMeteorites.GroupBy(m => m.Recclass).Select(gr => gr.Key).ToArray();

                 filter = new { years = years, classes = recclasses };

                _memoryCache.Set(cashKey, filter, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }
            return Ok(filter);
        }
    }
}