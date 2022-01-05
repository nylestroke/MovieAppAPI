using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using System.Diagnostics;
using MvcMovie.Services;

namespace MvcMovie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MovieService _movieService;

        public HomeController(ILogger<HomeController> logger, MovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(Request.Query["query"]))
            {
                var result = await this._movieService.GetMovies();
                return View(result);   
            }
            else
            {
                var result = await this._movieService.GetMovies(Request.Query["query"]);
                return View(result);
            }
        }

        public IActionResult Contact()
        {
            return View();
        }


        [Route("/movie/{id}")]
        public async Task<IActionResult> Movie(int id)
        {
            var result = await this._movieService.GetMovie(id);
            ViewData["id"] = id;
            return View("Movie", result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}