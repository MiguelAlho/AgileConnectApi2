using AgileConnectApi.IntegrationTests.Database;
using AgileConnectApi.Models.Repository;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Xunit;

namespace AgileConnectApi.IntegrationTests.Repositories
{
    public class SqlServerParticipantRepositoryTests : IClassFixture<DbTestFixture>
    {
        readonly DbTestFixture _fixture;
        readonly IOptions<DbConfiguration> _config;


        public SqlServerParticipantRepositoryTests(DbTestFixture fixture)
        {
            _fixture = fixture;

            _config = new OptionsManager<DbConfiguration>(new[]
            {
                new ConfigureOptions<DbConfiguration>(configuration =>
                    configuration.ConnectionString = _fixture.ConnectionString),
            });
        }

        [Fact]
        public void CanReadEmptyParticipantListWhenNoDataExists()
        {
            //arrange
            _fixture.ClearRecords();
            var repo = new SqlServerParticipantRepository(_config);

            //act
            var result = repo.GetListOfParticipants();

            //assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void CanReadParticipantListFromDatabase()
        {
            //arrange
            _fixture.ClearRecords();
            AddMockParticipantsToDatabase();

            var repo = new SqlServerParticipantRepository(_config);

            var result = repo.GetListOfParticipants();

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            Assert.Equal(2, result.Count());

            var array = result.OrderBy(o => o.Id).ToArray();
            Assert.Equal(id1, array[0].Id);
            Assert.Equal(name1, array[0].Name);
        }

       
        Guid id1 = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        Guid id2 = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);
        string name1 = "Name One";
        string name2 = "Name Two";

        private void AddMockParticipantsToDatabase()
        {
            using (var connection = _fixture.GetNewOpenConnection())
            {
                var insert = "Insert Into Participant (Id, Name) Values (@id, @name)";
                connection.Execute(insert, new { id = id1, name = name1 });
                connection.Execute(insert, new { id = id2, name = name2 });
            }
        }
    }
}
