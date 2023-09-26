using AutoMapper;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AuthorReadOnlyDto, AuthorUpdateDto>().ReverseMap();
            CreateMap<AuthorDetailsDto, AuthorUpdateDto>().ReverseMap();
            CreateMap<BookDetailsDto, BookUpdateDto>().ReverseMap();
        }
    }
}
