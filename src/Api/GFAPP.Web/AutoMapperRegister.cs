using AutoMapper;
using GFAPP.Model.Authentication.Users;
using GFAPP.Model.Authentication.Users.Dtos;
using GFAPP.Model.Company;
using GFAPP.Model.Company.Dtos;
using GFAPP.Model.EleDuplicate;
using GFAPP.Model.EleDuplicate.Dtos;
using GFAPP.Model.Enums;
using GFAPP.Model.IndSludge;
using GFAPP.Model.Record;
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
                cfg.CreateMap<EleDuplicateInfo, EleDuplicateDetail>()
                .ForMember(x => x.CeneratedOperatorSignBase64, src => src.MapFrom(y => y.CeneratedOperatorSign.ImgBase64))
                .ForMember(x => x.ProcessedOperatorSignBase64, src => src.MapFrom(y => y.ProcessedOperatorSign.ImgBase64))
                .ForMember(x => x.GeneratedCompanyName, src => src.MapFrom(y => y.GeneratedCompany.Name))
                .ForMember(x => x.GeneratedContact, src => src.MapFrom(y => y.GeneratedCompany.Contact))
                .ForMember(x => x.GeneratedTelephoneNumber, src => src.MapFrom(y => y.GeneratedCompany.TelephoneNumber))
                .ForMember(x => x.CarryingCompanyName, src => src.MapFrom(y => y.CarryingCompany.Name))
                .ForMember(x => x.CarryingContact, src => src.MapFrom(y => y.CarryingCompany.Contact))
                .ForMember(x => x.CarryingTelephoneNumber, src => src.MapFrom(y => y.CarryingCompany.TelephoneNumber))
                .ForMember(x => x.ProcessedCompanyName, src => src.MapFrom(y => y.ProcessedCompany.Name))
                .ForMember(x => x.ProcessedContact, src => src.MapFrom(y => y.ProcessedCompany.Contact))
                .ForMember(x => x.ProcessedTelephoneNumber, src => src.MapFrom(y => y.ProcessedCompany.TelephoneNumber))
                .ForMember(x => x.CityName, src => src.MapFrom(y => y.GeneratedCompany.City.Name));
                ;
                cfg.CreateMap<EleDuplicateCreateDto, EleDuplicateInfo>();
                cfg.CreateMap<EleDuplicateInfo, EleDuplicateCreateDto>()
                .ForMember(x => x.CeneratedOperatorSignBase64, src => src.MapFrom(y => y.CeneratedOperatorSign.ImgBase64))
                .ForMember(x => x.CarryingCompanyName, src => src.MapFrom(y => y.CarryingCompany.Name))
                .ForMember(x => x.ProcessedCompanyName, src => src.MapFrom(y => y.ProcessedCompany.Name))
                .ForMember(x => x.CityName, src => src.MapFrom(y => y.GeneratedCompany.City.Name));

                cfg.CreateMap<EleDuplicateInfo,  EleDuplicateAcceptDto> ()
                .ForMember(x => x.GeneratedCompany, src => src.MapFrom(y => y.GeneratedCompany.Name));
                cfg.CreateMap<EleDuplicateAcceptDto, EleDuplicateInfo>()
                .ForMember(x => x.Quantity, y => y.Ignore())
                .ForMember(x => x.GeneratedCompany, y => y.Ignore())
                .ForMember(x => x.QuantityPercentageOfMoisture, y => y.Ignore())
                .ForMember(x => x.Code, y => y.Ignore());

                cfg.CreateMap<EleDuplicateInfo, EleDuplicateMissionDto>()
                .ForMember(x=>x.GeneratedCompanyName, src=>src.MapFrom(y=>y.GeneratedCompany.Name))
                .ForMember(x=>x.CarryingCompanyName, src=>src.MapFrom(y=>y.CarryingCompany.Name))
                .ForMember(x=>x.ProcessedCompanyName, src=>src.MapFrom(y=>y.ProcessedCompany.Name))
                 .ForMember(x => x.CityName, src => src.MapFrom(y => y.GeneratedCompany.City.Name));

                cfg.CreateMap<RecordInfo, RecordDto>();

                //企业
                cfg.CreateMap<CompanyInfo, CompanyDto>()
                .ForMember(x => x.CityName, src => src.MapFrom(y => y.City.Name));
            });
        }
    }
}
