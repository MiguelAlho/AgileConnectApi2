using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileConnectApi.Models.Repository
{
    public class InMemoryParticipantRepository : IParticipantRepository
    {
        public IEnumerable<Participant> GetListOfParticipants()
        {
            return new List<Participant>(){
                new Participant(Guid.NewGuid(), "Miguel Alho"),
                new Participant(Guid.NewGuid(), "Eduardo Piairo"),
            };
        }
    }
}
