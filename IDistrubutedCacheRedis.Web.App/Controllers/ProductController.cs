using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistrubutedCacheRedis.Web.App.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDistributedCache _distrubutedCache;

        public ProductController(IDistributedCache distrubutedCache)
        {
            _distrubutedCache = distrubutedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();

            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(2);

            cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(60);

            _distrubutedCache.SetString("name", "john", cacheOptions);
            await _distrubutedCache.SetStringAsync("surname", "car", cacheOptions);

            return View();
        }

        public async Task<IActionResult> ShowCache()
        {
            ViewBag.Name = _distrubutedCache.GetString("name");
            ViewBag.Name = await _distrubutedCache.GetStringAsync("car");

            return View();
        }

        public async Task<IActionResult> RemoveCache()
        {
            _distrubutedCache.Remove("name");
            await _distrubutedCache.RemoveAsync("car");

            return View();
        }
    }
}
