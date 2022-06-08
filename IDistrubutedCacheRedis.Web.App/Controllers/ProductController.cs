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
        public IActionResult Index()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();

            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(2);

            cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(60);

            _distrubutedCache.SetString("name", "john", cacheOptions);

            return View();
        }

        public IActionResult ShowCache()
        {
            ViewBag.Name = _distrubutedCache.GetString("name");

            return View();
        }

        public IActionResult RemoveCache()
        {
            _distrubutedCache.Remove("name");

            return View();
        }
    }
}
