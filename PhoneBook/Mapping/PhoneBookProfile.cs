using System.Linq;
using AutoMapper;
using PhoneBook.Bll.Models;
using PhoneBook.Dal.Models;

namespace PhoneBook.Mapping;

public class PhoneBookProfile : Profile
{
    public PhoneBookProfile()
    {
        CreateMap<GroupDb, GroupDto>(MemberList.Destination);

        CreateMap<UserDb, UserData>(MemberList.Destination)
            .ForMember(x => x.PhoneNumbers, e => e.MapFrom(x => x.Phones));
        CreateMap<AddressDb, AddressDto>(MemberList.Destination);
        CreateMap<PhoneDataDb, PhoneData>(MemberList.Destination);
        CreateMap<PhoneCategoryDb, PhoneCategoryDto>(MemberList.Destination);

        CreateMap<SaveGroupRequest, GroupDb>(MemberList.Source);
        CreateMap<SaveCategoryRequest, PhoneCategoryDb>(MemberList.Source);

        CreateMap<UserDb, UserShortDataDto>(MemberList.Destination)
            .ForMember(x => x.FullName, expression => expression.MapFrom(x => x.Name))
            .ForMember(x => x.UserId, expression => expression.MapFrom(x => x.Id));

        CreateMap<SaveUserRequest, UserDb>(MemberList.Source)
            .ForMember(x => x.Phones, expression => expression.MapFrom(x => x.PhoneNumbers))
            .ForSourceMember(x => x.GroupIds, expression => expression.DoNotValidate());
        CreateMap<AddressDto, AddressDb>(MemberList.Source);
        CreateMap<SavePhoneNumberDto, PhoneDataDb>(MemberList.Source)
            .ForMember(x => x.CategoryId, expression => expression.MapFrom(x => x.PhoneCategoryId));

        CreateMap<UserDb, ExportUserDataDto>(MemberList.Destination)
            .ForMember(x => x.PhoneNumbers, expression => expression.MapFrom(x => string.Join(", ", x.Phones.Select(p => p.PhoneNumber))))
            .ForMember(x => x.Address,
                expression => expression.MapFrom(x => x.Address))
            .ForMember(x => x.Groups, expression => expression.MapFrom(x => string.Join(", ", x.Groups.Select(p => p.Name))));
    }
}