using AutoMapper;
using NuGet.DependencyResolver;

namespace Task_Management_System_API_1.MapperProfile
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Task, TaskViewModel>().ReverseMap();
        }
    }
}
