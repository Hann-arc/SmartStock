using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.Dtos.Stock;
using SmartStockAI.models;
using AutoMapper;

namespace SmartStockAI.Mappers
{
    public class StockTransactionProfile : Profile
    {
        public StockTransactionProfile()
        {
            CreateMap<StockTransaction, ResStockTransactionDto>()
                .ForMember(dest => dest.ItemName,
                    opt => opt.MapFrom(src => src.Item != null ? src.Item.Name : null))
                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(src => src.UserId));

            CreateMap<ReqStockInDto, StockTransaction>();
            CreateMap<ReqStockOutDto, StockTransaction>();
        }

    }
}