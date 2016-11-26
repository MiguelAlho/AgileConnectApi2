using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileConnectApi.Models;
using Xunit;

namespace AgileConnectApi.UnitTests.ViewModels
{
    public class ParticipantTests
    {
        [Fact]
        public void CanCreateInstanceOfParticipant()
        {
            var guid = Guid.NewGuid();
            var name = "Mock Name";

            var participant = new Participant(guid, name);
            
            Assert.NotNull(participant);
            Assert.Equal(guid, participant.Id);
            Assert.Equal(name, participant.Name);
        }
        

        [Fact]
        public void ConstructorGuardsNullName()
        {
            Assert.Throws<ArgumentException>(() => new Participant(Guid.NewGuid(), null));
        }

        [Fact]
        public void ConstructorGuardsEmptyName()
        {
            Assert.Throws<ArgumentException>(() => new Participant(Guid.NewGuid(), " "));
        }
    }
}
