namespace CityInfo.API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int NumberOfPointOfInterst
        {
            get
            {
                return PointsOfInterest.Count;
            }
        }

        public ICollection<PointOfInterstsDto> PointsOfInterest { get; set; } = new List<PointOfInterstsDto>();
    }
}
