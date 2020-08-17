using movie_rental_system.core.Data;
using movie_rental_system.core.DTOs;
using movie_rental_system.core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace movie_rental_system.core.Services
{
    public class CategoriesService
    {
        protected readonly MovieRentalContext context;
        public CategoriesService(MovieRentalContext context)
        {
            this.context = context;
        }

        public async Task<CategoryDTO_Out> addCategoryAsync(CategoryDTO_In categoryDTO)
        {
            var category = new Category()
            {
                CategoryName = categoryDTO.CategoryName,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
            };
            this.context.Categories.Add(category);
            await this.context.SaveChangesAsync();
            return formatCategory(category);
        }


        //private methods 
        private CategoryDTO_Out formatCategory(Category category)
        {
            return new CategoryDTO_Out
            {
                Id = category.Id,
                Category = category.CategoryName
            };
        }
    }


}
