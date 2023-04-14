using AutoMapper;
using Final_Project.Models;
using Final_Project.Models.ViewModels;

namespace Final_Project;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PersonalTracker, PersonalTrackerVM>();
        CreateMap<PersonalTrackerVM, PersonalTracker> ();
    }
}
