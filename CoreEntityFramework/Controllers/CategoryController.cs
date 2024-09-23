﻿using CoreEntityFramework.Interfaces;
using CoreEntityFramework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreEntityFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        // dependency injection of service interfaces
        public CategoryController(ICategoryService CategoryService)
        {
            _categoryService = CategoryService;
        }


        /// <summary>
        /// Returns a list of Categories
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{CategoryDto},{CategoryDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Category/List -> [{CategoryDto},{CategoryDto},..]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> ListCategories()
        {
            // empty list of data transfer object CategoryDto
            IEnumerable<CategoryDto> CategoryDtos = await _categoryService.ListCategories();
            // return 200 OK with CategoryDtos
            return Ok(CategoryDtos);
        }

        /// <summary>
        /// Returns a single Category specified by its {id}
        /// </summary>
        /// <param name="id">The category id</param>
        /// <returns>
        /// 200 OK
        /// {CategoryDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Category/Find/1 -> {CategoryDto}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<CategoryDto>> FindCategory(int id)
        {
           
            var Category = await _categoryService.FindCategory(id);

            // if the category could not be located, return 404 Not Found
            if (Category == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Category);
            }
        }

        /// <summary>
        /// Updates a Category
        /// </summary>
        /// <param name="id">The ID of the category to update</param>
        /// <param name="CategoryDto">The required information to update the category (CategoryName, CategoryColor)</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: api/Category/Update/5
        /// Request Headers: Content-Type: application/json
        /// Request Body: {CategoryDto}
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpPut(template: "Update/{id}")]
        public async Task<ActionResult> UpdateCategory(int id, CategoryDto CategoryDto)
        {
            // {id} in URL must match CategoryId in POST Body
            if (id != CategoryDto.CategoryId)
            {
                //400 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _categoryService.UpdateCategory(CategoryDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            //Status = Updated
            return NoContent();

        }

        /// <summary>
        /// Adds a Category
        /// </summary>
        /// <param name="CategoryDto">The required information to add the category (CategoryName, CategoryColor)</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Category/Find/{CategoryId}
        /// {CategoryDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: api/Category/Add
        /// Request Headers: Content-Type: application/json
        /// Request Body: {CategoryDto}
        /// ->
        /// Response Code: 201 Created
        /// Response Headers: Location: api/Category/Find/{CategoryId}
        /// </example>
        [HttpPost(template: "Add")]
        public async Task<ActionResult<Category>> AddCategory(CategoryDto CategoryDto)
        {
            ServiceResponse response = await _categoryService.AddCategory(CategoryDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // returns 201 Created with Location
            return Created($"api/Category/FindCategory/{CategoryDto.CategoryId}",CategoryDto);
        }

        /// <summary>
        /// Deletes the Category
        /// </summary>
        /// <param name="id">The id of the category to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/Category/Delete/7
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            ServiceResponse response = await _categoryService.DeleteCategory(id);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();

        }

        

        /// <summary>
        /// Returns a list of Categories for a specific product by its {id}
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{CategoryDto},{CategoryDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Category/ListForProduct/3 -> [{CategoryDto},{CategoryDto},..]
        /// </example>
        [HttpGet(template: "ListForProduct/{id}")]
        public async Task<IActionResult> ListCategoriesForProduct(int id)
        {
            // empty list of data transfer object CategoryDto
            IEnumerable<CategoryDto> CategoryDtos = await _categoryService.ListCategoriesForProduct(id);
            // return 200 OK with CategoryDtos
            return Ok(CategoryDtos);
        }

        /// <summary>
        /// Unlinks a category from a product
        /// </summary>
        /// <param name="id">The id of the category</param>
        /// <param name="id">The id of the product</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/Category/Unlink?CategoryId=4&Productid=12
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("Unlink")]
        public async Task<ActionResult> Unlink(int categoryId, int productId)
        {
            ServiceResponse response = await _categoryService.UnlinkCategoryFromProduct(categoryId, productId);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();

        }

        /// <summary>
        /// Links a category from a product
        /// </summary>
        /// <param name="id">The id of the category</param>
        /// <param name="id">The id of the product</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/Category/Link?CategoryId=4&Productid=12
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("Link")]
        public async Task<ActionResult> Link(int categoryId, int productId)
        {
            ServiceResponse response = await _categoryService.LinkCategoryToProduct(categoryId, productId);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();

        }



    }
}