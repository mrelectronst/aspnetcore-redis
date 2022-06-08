using IDistrubutedCacheRedis.Web.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

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

            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(10);

            cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(120);

            //_distrubutedCache.SetString("name", "john", cacheOptions);
            //await _distrubutedCache.SetStringAsync("surname", "car", cacheOptions);

            var product = new Product { Id = 1, Name = "Pen1", Price = 10 };

            string jsonProduct = JsonConvert.SerializeObject(product);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            _distrubutedCache.Set("byteProduct1",byteProduct);

            //await _distrubutedCache.SetStringAsync("product:2", jsonProduct);

            return View();
        }

        public async Task<IActionResult> ShowCache()
        {
            ViewBag.Name = _distrubutedCache.GetString("name");

            ViewBag.Surname = await _distrubutedCache.GetStringAsync("car");

            Byte[] byteProduct = await _distrubutedCache.GetAsync("byteProduct1");

            string strProduct = Encoding.UTF8.GetString(byteProduct);

            var product = JsonConvert.DeserializeObject<Product>(strProduct);

            ViewBag.Product = product;

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
