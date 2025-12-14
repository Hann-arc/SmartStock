using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStockAI.Dtos.Item;
using SmartStockAI.Exceptions;
using SmartStockAI.Interfaces;
using SmartStockAI.models;

namespace SmartStockAI.services
{
    public class ItemService : IItemService
    {

        private readonly IItemRepository _repo;
        private readonly IAuditLogService _auditLogService;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public ItemService(
            IItemRepository repo,
            ICategoryRepository categoryRepo,
            IMapper mapper, IAuditLogService auditLogService)
        {
            _repo = repo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _auditLogService = auditLogService;
        }


        public async Task<ResItemDto> CreateAsync(ReqCreateItemDto dto, Guid currentUserId)
        {
            var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);
            if (category == null) throw new NotFoundException("Category", dto.CategoryId);

            var item = _mapper.Map<Item>(dto);
            item.Id = Guid.NewGuid();

            await _repo.AddAsync(item);

            await _auditLogService.LogActionAsync(
                currentUserId,
                "Create",
                "Item",
                item.Id,
                newValues: _mapper.Map<ResItemDto>(item)
            );

            item = await _repo.GetByIdAsync(item.Id);

            return _mapper.Map<ResItemDto>(item);

        }

        public async Task<IEnumerable<ResItemDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ResItemDto>>(items);
        }

        public async Task<ResItemDto?> GetByIdAsync(Guid id)
        {
            var item = await _repo.GetByIdAsync(id);

            if (item == null) throw new NotFoundException("Item", id);

            return _mapper.Map<ResItemDto>(item);
        }

        public async Task<bool> RestoreAsync(Guid id)
        {
            var item = await _repo.GetByIdIncludeDeleteAsync(id);

            if (item == null) throw new NotFoundException("Item", id);

            await _repo.RestoreAsync(id);

            return true;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var item = await _repo.GetByIdAsync(id);

            if (item == null) throw new NotFoundException("Item", id);

            await _repo.SoftDeleteAsync(id);

            return true;
        }

        public async Task<ResItemDto?> UpdateAsync(Guid id, ReqUpdateItemDto dto, Guid currentUserId)
        {
            var oldItem = await _repo.GetByIdAsync(id);

            if (oldItem == null) throw new NotFoundException("Item", id);

            var oldValues = _mapper.Map<ResItemDto>(oldItem);

            if (dto.CategoryId.HasValue)
            {
                var category = await _categoryRepo.GetByIdAsync(dto.CategoryId.Value);

                if (category == null)
                    throw new NotFoundException("Category", dto.CategoryId.Value);

                oldItem.CategoryId = dto.CategoryId.Value;
            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                oldItem.Name = dto.Name;
            }

            if (dto.Stock.HasValue)
            {
                oldItem.Stock = dto.Stock.Value;
            }

            if (dto.Price.HasValue)
            {
                oldItem.Price = dto.Price.Value;
            }

            if (dto.MinimumThreshold.HasValue)
            {
                oldItem.MinimumThreshold = dto.MinimumThreshold.Value;
            }

            await _repo.UpdateAsync(oldItem);

            var updatedItem = await _repo.GetByIdAsync(id);

            var newValues = _mapper.Map<ResItemDto>(updatedItem);

            await _auditLogService.LogActionAsync(
                currentUserId,
                "Update",
                "Item",
                id,
                oldValues,
                newValues
            );

            return newValues;

        }
    }
}