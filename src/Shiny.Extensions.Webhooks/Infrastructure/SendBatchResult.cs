using System;
using System.Collections.Generic;

namespace Shiny.Extensions.Webhooks.Infrastructure;


public record SendBatchResult(
    IList<WebHookRegistration> Successful,
    IList<(WebHookRegistration Registration, Exception Exception)> Errors
);