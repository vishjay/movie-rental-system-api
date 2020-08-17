using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using movie_rental_system.core.Data;
using movie_rental_system.core.DTOs;
using movie_rental_system.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movie_rental_system.core.Services
{
    public class MoviesService
    {
        protected readonly MovieRentalContext context;
        public MoviesService(MovieRentalContext context)
        {
            this.context = context;
        }
        public async Task<MovieDTO_Out> addMovieAsync(MovieDTO_In movieDTO)
        {
            Movie movie = new Movie()
            {
                Name = movieDTO.Name,
                MovieLanguage = movieDTO.Language,
                ImageURL = movieDTO.ImageURL,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            movie.Categories = movieDTO.CategoryIds.Select(categoryId => new MovieCategory
            {
                Movie = movie,
                Category = context.Categories.Where(c => c.Id == categoryId).FirstOrDefault()

            }).ToList();
            this.context.Movies.Add(movie);
            await this.context.SaveChangesAsync();
            return this.formatMovie(movie);
        }



        public async Task<List<MovieDTO_Out>> getMoviesAsync()
        {
            return await this.context.Movies
            .Include(movie => movie.Categories)
            .Select(movie => new MovieDTO_Out
            {
                Id = movie.Id,
                Name = movie.Name,
                Language = movie.MovieLanguage,
                ImageURL = movie.ImageURL,
                Categories = movie.Categories
                                  .Select(c => new CategoryDTO_Out
                                  {
                                      Id = c.Category.Id,
                                      Category = c.Category.CategoryName
                                  }).ToList()
            }).ToListAsync();
        }



        public async Task<MovieDTO_Out> getMovieAync(Guid id)
        {
            return  await this.context.Movies
            .Where(movie => movie.Id == id)
            .Include(movie => movie.Categories)
            .Select(movie => new MovieDTO_Out
            {
                Id = movie.Id,
                Name = movie.Name,
                Language = movie.MovieLanguage,
                ImageURL = movie.ImageURL,
                Categories = movie.Categories
                                  .Select(c => new CategoryDTO_Out
                                  {
                                      Id = c.Category.Id,
                                      Category = c.Category.CategoryName
                                  }).ToList()
            }).FirstOrDefaultAsync();
        }


        public async Task<List<MovieDTO_Out>> findMovieAsync(String searchTerm)
        {
            return await this.context.Movies
            .Where(movie => movie.Name.Contains(searchTerm))
            .Include(movie => movie.Categories)
            .Select(movie => new MovieDTO_Out
            {
                Id = movie.Id,
                Name = movie.Name,
                Language = movie.MovieLanguage,
                ImageURL = movie.ImageURL,
                Categories = movie.Categories
                                  .Select(c => new CategoryDTO_Out
                                  {
                                      Id = c.Category.Id,
                                      Category = c.Category.CategoryName
                                  }).ToList()
            }).ToListAsync();
        }

        //private methods 
        private MovieDTO_Out formatMovie(Movie movie)
        {
            return new MovieDTO_Out
            {
                Id = movie.Id,
                Name = movie.Name,
                Language = movie.MovieLanguage,
                ImageURL = movie.ImageURL,
                Categories = movie.Categories
                                  .Select(c => new CategoryDTO_Out
                                  {
                                      Id = c.Category.Id,
                                      Category = c.Category.CategoryName
                                  }).ToList()
            };
        }
    }
}
