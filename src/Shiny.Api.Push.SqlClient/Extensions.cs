using Microsoft.Data.SqlClient;


namespace Shiny.Api.Push.SqlClient
{
    public static class Extensions
    {
        public static async Task EnsureConnected(this SqlConnection conn)
        {
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync().ConfigureAwait(false);
        }


        public static async Task<int> ExecuteNonQuery(this SqlConnection conn, string sql, params SqlParameter[] parameters)
        {
            await conn.EnsureConnected().ConfigureAwait(false);
            return await conn
                .CreateCommand(sql, parameters)
                .ExecuteNonQueryAsync()
                .ConfigureAwait(false);
        }


        public static async Task<object?> ExecuteScalar(this SqlConnection conn, string sql, params SqlParameter[] parameters)
        {
            await conn.EnsureConnected().ConfigureAwait(false);
            var result = await conn
                .CreateCommand(sql, parameters)
                .ExecuteScalarAsync()
                .ConfigureAwait(false);

            return result;
        }


        public static async Task<SqlDataReader> GetDataReader(this SqlConnection conn, string sql, params SqlParameter[] parameters)
        {
            await conn.EnsureConnected().ConfigureAwait(false);
            var reader = await conn
                .CreateCommand(sql, parameters)
                .ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection)
                .ConfigureAwait(false);
           
            return reader;
        }


        static SqlCommand CreateCommand(this SqlConnection conn, string sql, params SqlParameter[] parameters)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            if (parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            return cmd;
        } 
    }
}
