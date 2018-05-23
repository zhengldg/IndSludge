using AutoMapper;
using GFAPP.Model.Authentication.Users;
using GFAPP.Model.Authentication.Users.Dtos;
using GFAPP.Model.Company;
using GFAPP.Model.Company.Dtos;
using GFAPP.Model.EleDuplicate;
using GFAPP.Model.EleDuplicate.Dtos;
using GFAPP.Model.Enums;
using GFAPP.Model.IndSludge;
using System;

namespace GFAPP.Web
{
    /// <summary>
    /// automapper注册
    /// </summary>
    public class AutoMapperRegister
    {
        public static void Start()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Enum, string>().ConvertUsing(src => src.GetEnumDisplay());

                // 用户
                cfg.CreateMap<UserRegisterInput, UserInfo>();
                cfg.CreateMap<UserInfo, UserInfoDto>();
                
                //电子联单
                cfg.CreateMap<EleDuplicateCreateDto, EleDuplicateInfo>().ReverseMap();
                cfg.CreateMap<EleDuplicateInfo, EleDuplicateMissionDto>()
                .ForMember(x=>x.GeneratedCompany, src=>src.MapFrom(y=>y.GeneratedCompany.Name))
                .ForMember(x=>x.CarryingCompany, src=>src.MapFrom(y=>y.CarryingCompany.Name))
                .ForMember(x=>x.ProcessedCompany, src=>src.MapFrom(y=>y.ProcessedCompany.Name))
                ;

                //企业
                cfg.CreateMap<CompanyInfo, CompanyDto>();
            });
        }
    }
}
