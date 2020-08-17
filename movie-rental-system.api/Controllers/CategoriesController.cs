using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movie_rental_system.core.Data;
using movie_rental_system.core.DTOs;
using movie_rental_system.core.Models;
using movie_rental_system.core.Services;

namespace movie_rental_system.api.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : APIBaseController
    {
        protected readonly CategoriesService service;
        public CategoriesController(CategoriesService service){
            this.service = service;
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO_Out>> addCategory(CategoryDTO_In categoryDTO)
        {
            var category = await this.service.addCategoryAsync(categoryDTO);
            return Ok(category);
        }
    }
}