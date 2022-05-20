using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoExample.Services;
using MongoExample.Models;

namespace MongoExample.Controllers
{
    [Route("api/[controller]")]
    public class MovielistController : Controller
    {
        private readonly MongoDBService _mongoDBService;

        public MovielistController (MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<List<MovieList>> Get()
        {
            return await _mongoDBService.GetMoviesAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MovieList movieList)
        {
            Console.WriteLine(movieList);
            await _mongoDBService.CreateMovieAsync(movieList);
            return CreatedAtAction(nameof(Get), new { id = movieList.Id }, movieList);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _mongoDBService.DeleteMovieAsync(id);
            return NoContent();
        }
    }
}
