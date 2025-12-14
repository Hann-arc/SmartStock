using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStockAI.Dtos.Stock;
using SmartStockAI.Exceptions;
using SmartStockAI.Interfaces;
using SmartStockAI.models;
using SmartStockAI.Repositories;

namespace SmartStockAI.Services
{
    public class StockTransactionService : IStockTransactionService
    {

        private readonly IItemRepository _itemRepo;
        private readonly IStockTransactionRepository _trxRepo;

        private readonly IAuditLogService _auditLogService;
        private readonly IMapper _mapper;

        public StockTransactionService(
            IItemRepository itemRepo,
            IStockTransactionRepository trxRepo,
            IMapper mapper,
            IAuditLogService auditLogService)
        {
            _itemRepo = itemRepo;
            _trxRepo = trxRepo;
            _mapper = mapper;
            _auditLogService = auditLogService;
        }

        public async Task<ResStockTransactionDto> StockInAsync(Guid userId, ReqStockInDto dto)
        {
            var item = await _itemRepo.GetByIdAsync(dto.ItemId);

            if (item == null)
            {
                throw new NotFoundException("Item", dto.ItemId);
            }

            item.Stock += dto.Quantity;

            await _itemRepo.UpdateAsync(item);

            var transaction = new StockTransaction
            {
                Id = Guid.NewGuid(),
                ItemId = item.Id,
                UserId = userId,
                Quantity = dto.Quantity,
                Type = TransactionType.In,
            };

            await _trxRepo.AddAsync(transaction);

            await _auditLogService.LogActionAsync(
                userId,
                "StockIn",
                "StockTransaction",
                transaction.Id,
                newValues: new { 
                itemId = dto.ItemId,
                quantity = dto.Quantity,
                Notes = "Stock in operation"
              }
            );
            return _mapper.Map<ResStockTransactionDto>(transaction);

        }

        public async Task<ResStockTransactionDto> StockOutAsync(Guid userId, ReqStockOutDto dto)
        {
            if (dto.Quantity <= 0)
                throw new ValidationException("Quantity must be greater than 0");

            var item = await _itemRepo.GetByIdAsync(dto.ItemId);
            if (item == null)
                throw new NotFoundException("Item", dto.ItemId);

            if (item.Stock < dto.Quantity)
                throw new BusinessRuleException($"Insufficient stock. Available: {item.Stock}, Requested: {dto.Quantity}");


            item.Stock -= dto.Quantity;
            await _itemRepo.UpdateAsync(item);

            var transaction = new StockTransaction
            {
                Id = Guid.NewGuid(),
                ItemId = item.Id,
                UserId = userId,
                Quantity = dto.Quantity,
                Type = TransactionType.Out,
                CreatedAt = DateTime.UtcNow
            };

            await _trxRepo.AddAsync(transaction);

            await _auditLogService.LogActionAsync(
                userId,
                "StockOut",
                "StockTransaction",
                transaction.Id,
                newValues: new { 
                ItemId = dto.ItemId,
                Quantity = dto.Quantity,
                Notes = "Stock in operation"
              }
            );

            return _mapper.Map<ResStockTransactionDto>(transaction);
        }

        public async Task<IEnumerable<ResStockTransactionDto>> GetItemHistoryAsync(Guid itemId, int page, int pageSize)
        {
            var item = await _itemRepo.GetByIdAsync(itemId);
            if (item == null)
                throw new NotFoundException("Item", itemId);


            var transactions = await _trxRepo.GetHistoryAsync(itemId, page, pageSize);


            return _mapper.Map<IEnumerable<ResStockTransactionDto>>(transactions);
        }

        public async Task<int> CountHistoryAsync(Guid itemId)
        {
            return await _trxRepo.CountAsync(itemId);
        }
    }
}