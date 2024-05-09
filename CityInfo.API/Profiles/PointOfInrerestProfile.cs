using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class PointOfInrerestProfile : Profile
    {
        public PointOfInrerestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterstsDto>();
            CreateMap<Models.CreatePointOfInterestDto, Entities.PointOfInterest>();
            CreateMap<Models.UpdatePointOfInterestDto, Entities.PointOfInterest>();
            CreateMap<Entities.PointOfInterest, Models.UpdatePointOfInterestDto>();
        }
    }
}
