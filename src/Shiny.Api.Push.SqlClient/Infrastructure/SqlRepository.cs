using System.Text;
using Microsoft.Data.SqlClient;
using Shiny.Api.Push.Management;


namespace Shiny.Api.Push.SqlClient.Infrastructure
{
    public class SqlRepository : IRepository
    {
        readonly string connectionString;
        public SqlRepository(string connectionString) => this.connectionString = connectionString;

        //Task<List<Models.NotificationRegistrationModel>> FindRegistrations(PushFilter filter)
        //{
        //    var query = this.data.Registrations.AsQueryable();
        //    if (!String.IsNullOrWhiteSpace(filter.UserId))
        //        query = query.Where(x => x.UserId == filter.UserId);

        //    if (filter.Platform != null)
        //        query = query.Where(x => x.Platform == filter.Platform);

        //    if (filter.DeviceToken != null)
        //        query = query.Where(x => x.DeviceToken == filter.DeviceToken);

        //    if ((filter.Tags?.Length ?? 0) > 0)
        //        query = query.Where(x => x.Tags.Any(y => filter.Tags!.Any(tag => y.Value == tag)));

        //    return query.ToListAsync();
        //}
        public async Task<IEnumerable<NotificationRegistration>> Get(PushFilter? filter)
        {
            var list = new List<NotificationRegistration>();
            var sb = new StringBuilder();

            sb.Append("SELECT FROM dbo.NotificationRegistrations");
            if (filter != null)
                sb.Append(" WHERE ");

            using (var conn = new SqlConnection(this.connectionString))
            {
                using (var reader = await conn.GetDataReader("").ConfigureAwait(false))
                {

                }
            }

            return list;
        }


        public async Task Remove(PushFilter filter)
        {
            using (var conn = new SqlConnection(this.connectionString))
            { 
                await conn
                    .ExecuteNonQuery(
                        DELETE_SQL,
                        new SqlParameter("@DeviceToken", filter.DeviceToken),
                        new SqlParameter("@Platform", filter.Platform)
                    )
                    .ConfigureAwait(false);
            }
        }


        public async Task Create(NotificationRegistration reg)
        {
            using (var conn = new SqlConnection(this.connectionString))
            {
                var id = await conn.ExecuteScalar("INSERT INTO dbo.NotificationRegistrations() VALUES ()");
                foreach (var tag in reg.Tags)
                {
                    await conn
                        .ExecuteNonQuery("INSERT INTO ")
                        .ConfigureAwait(false);

                }
            }
        }


        public async Task Update(NotificationRegistration reg)
        {
            using (var conn = new SqlConnection(this.connectionString))
            {
                await conn
                    .ExecuteNonQuery(
                        DELETE_ALL_TAGS_SQL,
                        new SqlParameter("@Platform", reg.Platform),
                        new SqlParameter("@DeviceToken", reg.DeviceToken)
                    )
                    .ConfigureAwait(false);
            }
        }


        static async Task InsertTags(SqlConnection conn, int regId, string[] tags)
        {
            if (tags == null)
                return;

            foreach (var tag in tags)
            {
                await conn
                    .ExecuteNonQuery(
                        INSERT_TAG_SQL,
                        new SqlParameter("@Id", regId),
                        new SqlParameter("@Value", tag)
                    )
                    .ConfigureAwait(false);
            }
        }

        const string INSERT_SQL = "INSERT INTO dbo.NotificationRegistrations() VALUES (); SELECT SCOPE_IDENTITY()";
        const string INSERT_TAG_SQL = @"
INSERT INTO 
    dbo.NotificationRegistrationTags(NotificationRegistrationId, Value) 
SELECT
    NotificationRegistrationId, 
    @Value
FROM
    dbo.NotificationRegistrations
WHERE
    Platform = @Platform AND
    DeviceToken = @DeviceToken";

        const string UPDATE_SQL = @"
UPDATE
    dbo.NotificationRegistrations
SET
    UserId = @UserId,
    DateExpiry = @DateExpiry
WHERE
    Platform = @Platform AND
    DeviceToken = @DeviceToken";

        const string DELETE_SQL = @"
DELETE FROM
    dbo.NotificationRegistrations
WHERE
    Platform = @Platform AND
    DeviceToken = @DeviceToken";

        const string DELETE_ALL_TAGS_SQL = @"
DELETE FROM 
    dbo.NotificationRegistrationTags 
WHERE 
    NotificationRegistrationId IN (
        SELECT
            NotificationRegistrationId
        FROM
            dbo.NotificationRegistrations
        WHERE
            Platform = @Platform AND
            DeviceToken = @DeviceToken
    )";
    }
}
