using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStockAI.Dtos.Category;
using SmartStockAI.Exceptions;
using SmartStockAI.Interfaces;
using SmartStockAI.models;

namespace SmartStockAI.services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ResCategoryDto> CreateAsync(ReqCreateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("Category name cannot be empty");

            var existing = await _repo.GetByNameAsync(dto.Name.Trim());
            if (existing != null)
                throw new DuplicateEntityException("Category", dto.Name);

            var category = _mapper.Map<Category>(dto);
            category.Id = Guid.NewGuid();
            category.Name = dto.Name.Trim();
            category.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(category);
            category = await _repo.GetByIdAsync(category.Id)
                ?? throw new NotFoundException("Category", category.Id);

            return _mapper.Map<ResCategoryDto>(category);
        }

        public async Task<IEnumerable<ResCategoryDto>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();

            return _mapper.Map<IEnumerable<ResCategoryDto>>(categories);
        }

        public async Task<ResCategoryDto?> GetByIdAsync(Guid id)
        {
            var category = await _repo.GetByIdAsync(id);

            if (category == null) throw new NotFoundException("Category", id);

            return _mapper.Map<ResCategoryDto>(category);
        }

        public async Task<bool> RestoreAsync(Guid id)
        {
            var category = await _repo.GetByIdIncludingDeletedAsync(id);

            Console.WriteLine($"{category}, category di service sebelum if");
            if (category == null)
                throw new NotFoundException("Category", id);

            Console.WriteLine($"{category}, category di service setelah if");

            if (!category.IsDeleted)
                throw new BusinessRuleException("Category is not deleted and cannot be restored");

            var existing = await _repo.GetByNameAsync(category.Name);
            if (existing != null)
                throw new DuplicateEntityException("Category", category.Name);

            await _repo.RestoreAsync(id);
            return true;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var category = await _repo.GetByIdIncludingDeletedAsync(id);

            if (category == null) throw new NotFoundException("Category", id);

            await _repo.SoftDeleteAsync(id);

            return true;
        }

        public async Task<ResCategoryDto?> UpdateAsync(Guid id, ReqUpdateCategoryDto dto)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException("Category", id);

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                var trimmedName = dto.Name.Trim();
                if (string.IsNullOrWhiteSpace(trimmedName))
                    throw new ValidationException("Category name cannot be empty");

                var existing = await _repo.GetByNameAsync(trimmedName);
                if (existing != null && existing.Id != id)
                    throw new DuplicateEntityException("Category", trimmedName);

                category.Name = trimmedName;
            }

            await _repo.UpdateAsync(category);
            category = await _repo.GetByIdAsync(id);

            return _mapper.Map<ResCategoryDto>(category);
        }
    }
}