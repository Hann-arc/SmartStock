using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SmartStockAI.Dtos.Alert;
using SmartStockAI.Exceptions;
using SmartStockAI.Interfaces;
using SmartStockAI.models;

namespace SmartStockAI.Services
{
    public class AlertService : IAlertService
    {

        private readonly IAlertRepository _alertRepo;
        private readonly IItemRepository _itemRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public AlertService(IAlertRepository alertRepo, IItemRepository itemRepo, UserManager<AppUser> userManager, IMapper mapper)
        {
            _alertRepo = alertRepo;
            _itemRepo = itemRepo;
            _userManager = userManager;
            _mapper = mapper;

        }

        public async Task CheckLowStockItemsAsync()
        {
            var systemUser = await GetSystemUserAsync();

            var items = await _itemRepo.GetAllAsync();

            foreach (var item in items)
            {
                if (item.Stock < item.MinimumThreshold)
                {
                    var existingAlert = await _alertRepo.GetActiveAlertByItemIdAsync(item.Id);

                    if (existingAlert == null)
                    {
                        var alert = new Alert
                        {
                            Id = Guid.NewGuid(),
                            ItemId = item.Id,
                            CreatedById = systemUser.Id,
                            Message = $"STOK RENDAH: {item.Name} - Stok: {item.Stock}, Threshold: {item.MinimumThreshold}",
                            CreatedAt = DateTime.UtcNow
                        };

                        await _alertRepo.AddAsync(alert);
                    }
                }
            }
        }

        public async Task<IEnumerable<ResAlertDto>> GetActiveAlertsAsync()
        {
            var alerts = await _alertRepo.GetActiveAlertsAsync();
            return _mapper.Map<IEnumerable<ResAlertDto>>(alerts);
        }

        public async Task<ResAlertDto> ResolveAlertAsync(ReqResolveAlertDto dto)
        {
            var alert = await _alertRepo.GetByIdAsync(dto.AlertId);
            if (alert == null)
                throw new NotFoundException("Alert", dto.AlertId);

            if (alert.Resolved)
                throw new BusinessRuleException("Alert already resolve before");


            alert.Resolved = true;
            await _alertRepo.UpdateAsync(alert);

            return _mapper.Map<ResAlertDto>(alert);
        }

        public async Task<bool> IsItemBelowThresholdAsync(Guid itemId)
        {
            var item = await _itemRepo.GetByIdAsync(itemId);
            return item != null && item.Stock < item.MinimumThreshold;
        }

        private async Task<AppUser> GetSystemUserAsync()
        {
            var systemUser = await _userManager.FindByEmailAsync("system@smartstock.ai");

            if (systemUser == null)
            {
                systemUser = new AppUser
                {
                    UserName = "system@smartstock.ai",
                    Email = "system@smartstock.ai",
                    FullName = "System User",
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(systemUser, "SystemUser123!");
                if (!createResult.Succeeded)
                {
                    throw new Exception($"Failed to create system user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                }
            }

            return systemUser;
        }

    }
}