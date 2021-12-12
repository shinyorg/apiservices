
using System.Runtime.CompilerServices;

namespace Shiny.Api.Push.GeoDispatching
{
    public class GeoDispatchNotification : Notification
    {
        public string Message
        {
            get => this.Data[nameof(this.Message)];
            set => this.Data[nameof(this.Message)] = value;
        }


        public double CenterLatitude
        {
            get => this.GetDouble();
            set => this.SetDouble(value);
        }


        public double CenterLongitude
        {
            get => this.GetDouble();
            set => this.SetDouble(value);
        }


        public double RadiusMeters
        {
            get => this.GetDouble();
            set => this.SetDouble(value);
        }


        double GetDouble([CallerMemberName] string? property = null)
        {
            return Double.Parse(this.Data[property] ?? "0");
        }

        void SetDouble(double value, [CallerMemberName] string? property = null)
        {
            this.Data[property] = value.ToString();
        }
    }
}
