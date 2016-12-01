using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using AgileConnectApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Moq;
using AgileConnectApi.Models;

namespace AgileConnectApi.UnitTests.Controllers
{
    public class ParticipantControllerTests
    {
        [Fact]
        public void CanCreateInstanceOfParticipantController()
        {
            Mock<IParticipantRepository> repo = new Mock<IParticipantRepository>();
            
            //act
            var controller = new ParticipantController(repo.Object);
            
            //assert
            Assert.NotNull(controller);
            Assert.IsAssignableFrom<Controller>(controller);
        }

        [Fact]
        public void NullRepositoryAtConstructorThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new ParticipantController(null));
        }


        [Fact]
        public void CanGetListOfParticipants()
        {
            Mock<IParticipantRepository> repo = new Mock<IParticipantRepository>();
            repo.Setup(x => x.GetListOfParticipants()).Returns(
                new List<Participant>(){
                    new Participant(Guid.NewGuid(), "Person", "A"),
                    new Participant(Guid.NewGuid(), "Person", "B")
                }
            );
            
            var controller = new ParticipantController(repo.Object);

            var result = controller.GetListOfParticipants();
            //we'll map this to a HTTPGet of \Participant\

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Person A", result.First().Name);
            Assert.Equal("Person", result.First().FirstName);
            Assert.Equal("A", result.First().LastName);
        }
    }
}

