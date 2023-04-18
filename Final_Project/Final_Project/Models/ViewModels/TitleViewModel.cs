using Microsoft.AspNetCore.Mvc.Rendering;

namespace Final_Project.Models.ViewModels
{
    public class TitleViewModel
    {
        public string TitleSearch { get; set; }
        public string Search { get; set; }
        public string CourseSearch { get; set; }
        public List<PersonalTracker> Trackers { get; set; }
        public SelectList? Titles { get; set; }

    }
}
