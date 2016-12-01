using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Options;

namespace AgileConnectApi.Models.Repository
{
    public class SqlServerParticipantRepository : IParticipantRepository
    {
        readonly string _connectionString;

        public SqlServerParticipantRepository(IOptions<DbConfiguration> dbOptions)
        {
            if (dbOptions == null)
                throw new ArgumentException(nameof(dbOptions));
            if (string.IsNullOrWhiteSpace(dbOptions.Value.ConnectionString))
                throw new ArgumentException("connectionsString");

            _connectionString = dbOptions.Value.ConnectionString;
        }

        public IEnumerable<Participant> GetListOfParticipants()
        {
            var queryText = "Select Id, FirstName, LastName From Participant";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<Participant>(queryText);
            }
        }
    }
}
