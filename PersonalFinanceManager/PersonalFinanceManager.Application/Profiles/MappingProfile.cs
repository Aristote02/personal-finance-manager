using AutoMapper;
using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Shared.Requests.Auth;

namespace PersonalFinanceManager.Application.Profiles;

public class MappingProfile : Profile
{
    /// <summary>
    /// Mapping configurations
    /// </summary>
    public MappingProfile()
    {
        CreateMap<UserRegisterRequest, AppUser>()
            .ReverseMap();
    }
}