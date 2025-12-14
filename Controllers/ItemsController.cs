using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Dtos.Item;
using SmartStockAI.Extensions;
using SmartStockAI.Interfaces;

namespace SmartStockAI.Controllers
{
    [Route("api/items")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Get all items (Admin & Staff).
        /// </summary>
        /// <returns>List of items</returns>
        /// <response code="200">Returns all items</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Forbidden - missing required role</response>
        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        [ProducesResponseType(typeof(IEnumerable<ResItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemService.GetAllAsync();
            return Ok(items);
        }

        /// <summary>
        /// Get item by ID (Admin & Staff).
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns>Item details</returns>
        /// <response code="200">Returns the item</response>
        /// <response code="404">Item not found</response>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Staff")]
        [ProducesResponseType(typeof(ResItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _itemService.GetByIdAsync(id);

            return Ok(item);
        }

        /// <summary>
        /// Create a new item (Admin only).
        /// </summary>
        /// <param name="dto">Item creation data</param>
        /// <returns>Newly created item</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Category not found</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] ReqCreateItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!User.TryGetUserId(out var userId))
            return Unauthorized("Invalid user token");
        

            var created = await _itemService.CreateAsync(dto, userId);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update item (Admin only).
        /// </summary>
        /// <param name="id">Item ID to update</param>
        /// <param name="dto">Update data</param>
        /// <returns>Updated item</returns>
        /// <response code="200">Returns the updated item</response>
        /// <response code="404">Item or category not found</response>
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReqUpdateItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!User.TryGetUserId(out var userId))
            return Unauthorized("Invalid user token");

            var updated = await _itemService.UpdateAsync(id, dto,userId);

            return Ok(updated);
        }

        /// <summary>
        /// Soft delete item (Admin only).
        /// </summary>
        /// <param name="id">Item ID to delete</param>
        /// <returns>Success message</returns>
        /// <response code="200">Item deleted successfully</response>
        /// <response code="404">Item not found</response>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            await _itemService.SoftDeleteAsync(id);

            return Ok(new { message = "Item deleted" });
        }

        /// <summary>
        /// Restore deleted item (Admin only).
        /// </summary>
        /// <param name="id">Item ID to restore</param>
        /// <returns>Success message</returns>
        /// <response code="200">Item restored successfully</response>
        /// <response code="404">Item not found or not deleted</response>
        [HttpPatch("{id:guid}/restore")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Restore(Guid id)
        {
            await _itemService.RestoreAsync(id);

            return Ok(new { message = "Item restored" });
        }
    }
}