using System;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push
{
	public static class PushManagerExtensions
	{
		public static Task SendToUser(this IPushManager pushManager, Notification notification, string userId, CancellationToken cancelToken = default)
			=> pushManager.Send(notification, new PushFilter { UserId = userId }, cancelToken);
	}
}

