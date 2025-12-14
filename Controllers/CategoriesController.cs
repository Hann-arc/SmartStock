using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Dtos.Category;
using SmartStockAI.Interfaces;

namespace SmartStockAI.Controllers
{

    [Route("api/categories")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get all Categories (Admin & Staff).
        /// </summary>
        /// <returns>List of Categories</returns>
        /// <response code="200">Returns all Categories</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Forbidden - missing required role</response>
        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        [ProducesResponseType(typeof(IEnumerable<ResCategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var items = await _categoryService.GetAllAsync();
            return Ok(items);
        }

        /// <summary>
        /// Get Category by ID (Admin & Staff).
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category details</returns>
        /// <response code="200">Returns the Category</response>
        /// <response code="404">Category not found</response>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Staff")]
        [ProducesResponseType(typeof(ResCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            return Ok(category);
        }

        /// <summary>
        /// Create a new Category (Admin only).
        /// </summary>
        /// <returns>Newly created Category</returns>
        /// <response code="201">Returns the newly created Category</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResCategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] ReqCreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _categoryService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update category (Admin only).
        /// </summary>
        /// <param name="id">category ID to update</param>
        /// <param name="dto">Update data</param>
        /// <returns>Updated category</returns>
        /// <response code="200">Returns the updated category</response>
        /// <response code="404">category not found</response>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] 
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReqUpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _categoryService.UpdateAsync(id, dto);

            return Ok(updated);
        }

        /// <summary>
        /// Soft delete Category (Admin only).
        /// </summary>
        /// <param name="id">Category ID to delete</param>
        /// <returns>Success message</returns>
        /// <response code="200">Category deleted successfully</response>
        /// <response code="404">Category not found</response>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            await _categoryService.SoftDeleteAsync(id);

            return Ok(new { message = "Category deleted" });
        }

        /// <summary>
        /// Restore deleted Category (Admin only).
        /// </summary>
        /// <param name="id">Category ID to restore</param>
        /// <returns>Success message</returns>
        /// <response code="200">Category restored successfully</response>
        /// <response code="404">Category not found or not deleted</response>
        [HttpPatch("{id:guid}/restore")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] 
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Restore(Guid id)
        {
            await _categoryService.RestoreAsync(id);

            return Ok(new { message = "Category restored" });
        }

    }

}