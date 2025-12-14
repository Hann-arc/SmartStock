using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using SmartStockAI.Dtos.AuditLog;
using SmartStockAI.Interfaces;
using SmartStockAI.models;

namespace SmartStockAI.Services
{
    public class AuditLogService : IAuditLogService
    {

        private readonly IAuditLogRepository _repo;
        private readonly IMapper _mapper;

        public AuditLogService(IAuditLogRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<PagedResult<ResAuditLogDto>> GetLogsAsync(DateTime? startDate = null, DateTime? endDate = null, Guid? userId = null, string? entityType = null, string? action = null, int page = 1, int pageSize = 10)
        {
            var result = await _repo.GetLogsAsync(startDate, endDate, userId, entityType, action, page, pageSize);

            var mappedItems = result.Items.Select(a => new ResAuditLogDto
            {
                Id = a.Id,
                UserName = a.User?.FullName ?? "Unknown User",
                UserEmail = a.User?.Email ?? "Unknown Email",
                Action = a.Action,
                EntityType = a.EntityType,
                EntityId = a.EntityId,
                OldValues = a.OldValues,
                NewValues = a.NewValues,
                Timestamp = a.TimeStemp
            }).ToList();

            return new PagedResult<ResAuditLogDto>(mappedItems, result.TotalCount, page, pageSize);
        }

        public async Task LogActionAsync<T>(Guid userId, string action, string entityType, Guid entityId, T oldValues = default, T newValues = default)
        {
            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                OldValues = oldValues != null ? JsonSerializer.Serialize(oldValues) : null,
                NewValues = newValues != null ? JsonSerializer.Serialize(newValues) : null,
                TimeStemp = DateTime.UtcNow
            };

            await _repo.AddAsync(auditLog);
        }

    }
}