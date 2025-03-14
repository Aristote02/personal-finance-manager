using AutoMapper;
using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Shared.DTOs.Incomes;
using PersonalFinanceManager.Shared.RequestFeatures;
using PersonalFinanceManager.Shared.Requests.Auth;
using PersonalFinanceManager.Shared.Requests.Incomes;

namespace PersonalFinanceManager.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegisterRequest, AppUser>()
            .ReverseMap();

        CreateMap<IncomeCreationRequest, Income>()
            .ReverseMap();
        CreateMap<IncomeUpdateRequest, Income>();
        CreateMap<Income, IncomeResponseDto>();
        CreateMap<PagedList<Income>, IncomesResponse>()
            .ForMember(dest => dest.MetaData, opt => opt.MapFrom(src => src.MetaData))
            .ConstructUsing((src, context) => new IncomesResponse(src.Select(income =>
                context.Mapper.Map<IncomeResponseDto>(income)).ToList(), src.MetaData));
    }
}