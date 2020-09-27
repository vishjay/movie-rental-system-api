using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_rental_system.api.Middleware;
using movie_rental_system.core.Data;
using movie_rental_system.core.DTOs;
using movie_rental_system.core.Models;
using movie_rental_system.core.Services;

namespace movie_rental_system.api.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(APIExceptionFilter))]
    [Route("api/[controller]")]
    public class MoviesController : APIBaseController
    {
        protected readonly MoviesService service;
        public MoviesController (MoviesService service) {
            this.service = service;
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> addMovie(MovieDTO_In movieDTO) {

            var movie = await this.service.addMovieAsync(movieDTO);
            return CreatedAtAction(nameof(getMovie), new { id = movie.Id }, movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<MovieDTO_Out>> getMovies()
        {
            return await this.service.getMoviesAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieDTO_Out>> getMovie(Guid id)
        {
            var movie = await this.service.getMovieAync(id);
            if(movie == null)
            {
                return NotFound();
            }
            return movie;
        }


        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<MovieDTO_Out>> findMovies(String searchTerm)
        {
            return await this.service.findMovieAsync(searchTerm);
        }

        [HttpGet("error-test")]
        public void TestException()
        {
            throw new InvalidOperationException("This is an unhandled exception");
        }

    }
}