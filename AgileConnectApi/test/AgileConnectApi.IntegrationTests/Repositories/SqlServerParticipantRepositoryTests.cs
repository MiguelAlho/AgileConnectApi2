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
            AddMockParticipantsToDatabaseThroughSplitNames();

            var repo = new SqlServerParticipantRepository(_config);

            var result = repo.GetListOfParticipants();

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            Assert.Equal(2, result.Count());

            var array = result.OrderBy(o => o.Id).ToArray();
            Assert.Equal(id1, array[0].Id);
            Assert.Equal(name1, array[0].Name);
            Assert.Equal(name1FirstName, array[0].FirstName);
            Assert.Equal(name1LastName, array[0].LastName);
        }

       
        Guid id1 = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        Guid id2 = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);
        string name1 = "Name One";
        string name2 = "Name Two";
        string name1FirstName = "Name";
        string name2FirstName = "Name";
        string name1LastName = "One";
        string name2LastName = "Two";

        private void AddMockParticipantsToDatabaseThroughSplitNames()
        {
            using (var connection = _fixture.GetNewOpenConnection())
            {
                var insert = "Insert Into Participant (Id, FirstName, LastName) Values (@id, @firstname, @lastName)";
                connection.Execute(insert, new { id = id1, firstname = name1FirstName, lastname = name1LastName });
                connection.Execute(insert, new { id = id2, firstname = name2FirstName, lastname = name2LastName });
            }
        }

    }
}
