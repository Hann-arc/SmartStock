using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStockAI.Dtos.Category;
using SmartStockAI.models;

namespace SmartStockAI.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, ResCategoryDto>();
            CreateMap<ReqCreateCategoryDto, Category>();
            CreateMap<ReqUpdateCategoryDto, Category>()
            .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));
        }
    }
}