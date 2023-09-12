using AutoMapper;
using FirstCodingExam.Dto;
using FirstCodingExam.Models;

namespace FirstCodingExam.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistration, User>()
                .ForMember(UserRegistration => UserRegistration.Firstname,
                Option => Option.MapFrom(User => User.Firstname))
                .ForMember(UserRegistration => UserRegistration.Lastname,
                Option => Option.MapFrom(User => User.Lastname))
                .ForMember(UserRegistration => UserRegistration.Email,
                Option => Option.MapFrom(User => User.Email))
                .ForMember(UserRegistration => UserRegistration.Password,
                Option => Option.MapFrom(User => User.Password));
            CreateMap<NewRecord, Record>();
            CreateMap<Record, NewRecord>();
            CreateMap<HistoryRecord, Record>();
            CreateMap<Record, HistoryRecord>()
                .ForMember(Record => Record.RecordId,
                Option => Option.MapFrom(HistoryRecord => HistoryRecord.Id));
        }
    }
}
