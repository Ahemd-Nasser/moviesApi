using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesApi.Data;
using moviesApi.Services;

namespace moviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;

        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var Genres = await _genresService.GetAll();
            return Ok(Genres);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto dto)
        {
            var genre = new Genre {Name = dto.Name };
            await _genresService.Add(genre);
            return Ok(genre);
        }
        [HttpPut("{id}")]
        // api/Genres/1
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] GenreDto dto)
        {
            var genre = await _genresService.GetById(id);

            if(genre == null)
            {
                return NotFound($"No genre was found with Id: {id}");
            }

            genre.Name = dto.Name;

            _genresService.Update(genre);
            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _genresService.GetById(id);

            if (genre == null)
            {
                return NotFound($"No genre was found with Id: {id}");
            }

            _genresService.Delete(genre);

            return Ok(genre);

        }

    }
}
