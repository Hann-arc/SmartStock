using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStockAI.Dtos.Alert;
using SmartStockAI.models;

namespace SmartStockAI.Mappers
{
    public class AlertProfile :Profile
    {
        public AlertProfile()
        {
            CreateMap<Alert, ResAlertDto>()
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name))
            .ForMember(dest => dest.CreatedByEmail, opt => opt.MapFrom(src => src.CreatedBy.Email));
        }
    }
}