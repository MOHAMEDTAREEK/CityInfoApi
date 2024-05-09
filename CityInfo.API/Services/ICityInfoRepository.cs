using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAysnc();
        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAysnc(
            string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<City?> GetCityAysnc(int cityId, bool includePointOfInterest);
        public  Task<bool> CityExistsAysnc(int cityId);
        Task<IEnumerable<PointOfInterest>> GetPointOfInterests(int cityId);
        Task<PointOfInterest?> GetPointOfInterest(int cityId, int pointOfInterestId);
        Task AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAysnc();
    }
}
