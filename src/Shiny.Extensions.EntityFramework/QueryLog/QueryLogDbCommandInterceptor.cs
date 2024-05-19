using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Shiny.QueryLog;


public class QueryLogDbCommandInterceptor(IContextInfoProvider infoProvider, TimeSpan minLogDuration) : IDbCommandInterceptor
{
    public int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
    {
        this.TryLog(command, eventData);
        return result;
    }
    

    public DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        this.TryLog(command, eventData);
        return result;
    }


    public object? ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object? result)
    {
        this.TryLog(command, eventData);
        return result;
    }


    protected void TryLog(DbCommand command, CommandExecutedEventData eventData)
    {
        if (minLogDuration < eventData.Duration)
        {
            var log = new QueryLogEntry
            {
                UserIdentifier = infoProvider.UserIdentifier,
                UserIpAddress = infoProvider.UserIpAddress,
                AppLocation = infoProvider.AppLocation,
                
                Query = eventData.Command.CommandText,
                Duration = eventData.Duration,
                Timestamp = DateTimeOffset.UtcNow
            };
            try
            {
                // TODO: cannot audit seeding/migrations data
                eventData.Context!.Add(log);
                eventData.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }        
    }
}