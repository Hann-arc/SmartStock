using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStockAI.Dtos.Item;
using SmartStockAI.models;

namespace SmartStockAI.Mappers
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            // Create → Entity
            CreateMap<ReqCreateItemDto, Item>();

            // Update → Entity (patch style)
            CreateMap<ReqUpdateItemDto, Item>()
            .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));

             // Entity → Response

             CreateMap<Item, ResItemDto>()
             .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
           }

    }
}