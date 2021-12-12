namespace Shiny.Api.Push.GeoDispatching
{
    public static class PushManagerExtensions
    {
        public static Task SendGeoDispatch(this IPushManager pushManager, GeoDispatchNotification notification, PushFilter? filter)
        {
            // TODO: validate message
            //if (notification.CenterLatitude)
            //{

            //}
            return pushManager.Send(notification, filter);
        }
    }
}