
namespace Shiny.Api.Push.GeoDispatching
{
    public class GeoDispatchNotification
    {
        public string Message { get; set; }
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }
        public double RadiusMeters { get; set; }
    }
}
