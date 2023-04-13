using AutoMapper;
using Final_Project.Models;
using Final_Project.Models.ViewModels;

namespace Final_Project;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Personal_Tracker, Personal_TrackerVM>();
        CreateMap<Personal_TrackerVM, Personal_Tracker> ();
    }
}
