using AutoMapper;
using Final_Project.Models;
using Final_Project.Models.ViewModels;

namespace Final_Project;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PersonalTracker, Personal_TrackerVM>();
        CreateMap<Personal_TrackerVM, PersonalTracker> ();
    }
}
