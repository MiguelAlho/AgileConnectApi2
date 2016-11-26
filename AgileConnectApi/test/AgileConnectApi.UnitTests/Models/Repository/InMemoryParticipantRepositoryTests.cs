using AgileConnectApi.Models;
using AgileConnectApi.Models.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AgileConnectApi.UnitTests.Models.Repository
{
    public class InMemoryParticipantRepositoryTests
    {
        [Fact]
        public void CanCreateInstanceOfInMemoryParticipantRepository()
        {
            var sut = new InMemoryParticipantRepository();

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<IParticipantRepository>(sut);
        }

    }
}
