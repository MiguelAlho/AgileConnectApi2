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
            var firstname = "Mock";
            var lastname = "Name";

            var participant = new Participant(guid, firstname, lastname );
            
            Assert.NotNull(participant);
            Assert.Equal(guid, participant.Id);
            Assert.Equal(name, participant.Name);
            Assert.Equal(firstname, participant.FirstName);
            Assert.Equal(lastname, participant.LastName);
        }
        

        [Fact]
        public void ConstructorGuardsNullFirstName()
        {
            Assert.Throws<ArgumentException>(() => new Participant(Guid.NewGuid(), null, "Last"));
        }

        [Fact]
        public void ConstructorGuardsNullLastName()
        {
            Assert.Throws<ArgumentException>(() => new Participant(Guid.NewGuid(), "First", null));
        }

        [Fact]
        public void ConstructorGuardsEmptyFirstName()
        {
            Assert.Throws<ArgumentException>(() => new Participant(Guid.NewGuid(), " ", "Last"));
        }

        [Fact]
        public void ConstructorGuardsEmptyLastName()
        {
            Assert.Throws<ArgumentException>(() => new Participant(Guid.NewGuid(), "First", " "));
        }
    }
}
