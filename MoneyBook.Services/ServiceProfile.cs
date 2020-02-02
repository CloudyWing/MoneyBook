using AutoMapper;
using MoneyBook.Repositories;
using MoneyBook.Services.CategoryItemModel;
using MoneyBook.Services.CategoryModel;
using MoneyBook.Services.RecordModel;

namespace MoneyBook.Services {
    /// <summary>
    /// Build a Mapping between Service Data Transfer Objects and Entities Rule
    /// </summary>
    internal class ServiceProfile : Profile {
        public ServiceProfile() {
            CreateCategoryRule();
            CreateCategoryItemRule();
            CreateRecordRule();
        }

        private void CreateCategoryRule() {
            CreateMap<CategoryDto, Category>()
                .ForMember(x => x.PayType, opt => opt.MapFrom(x => x.PayType.Value))
                .ForMember(x => x.SeqNo, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.UserId, opt => opt.Ignore())
                .ForMember(x => x.IsDisabled, opt => opt.Ignore())
                .ForMember(x => x.SortNumber, opt => opt.Ignore())
                .ForMember(x => x.CreatedTime, opt => opt.Ignore())
                .ForMember(x => x.ModifiedTime, opt => opt.Ignore())
                .ForMember(x => x.DeletedTime, opt => opt.Ignore())
                .ForMember(x => x.CategoryItems, opt => opt.Ignore())
                .ForMember(x => x.AspNetUser, opt => opt.Ignore());
        }

        private void CreateCategoryItemRule() {
            CreateMap<CategoryItemDto, CategoryItem>()
                .ForMember(x => x.SeqNo, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.IsDisabled, opt => opt.Ignore())
                .ForMember(x => x.SortNumber, opt => opt.Ignore())
                .ForMember(x => x.CreatedTime, opt => opt.Ignore())
                .ForMember(x => x.ModifiedTime, opt => opt.Ignore())
                .ForMember(x => x.DeletedTime, opt => opt.Ignore())
                .ForMember(x => x.Category, opt => opt.Ignore())
                .ForMember(x => x.Records, opt => opt.Ignore());
        }

        private void CreateRecordRule() {
            CreateMap<RecordDto, Record>()
                .ForMember(x => x.SeqNo, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedTime, opt => opt.Ignore())
                .ForMember(x => x.ModifiedTime, opt => opt.Ignore())
                .ForMember(x => x.DeletedTime, opt => opt.Ignore())
                .ForMember(x => x.CategoryItem, opt => opt.Ignore());
        }
    }
}
