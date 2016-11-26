using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileConnectApi.Models
{
    public interface IParticipantRepository
    {
        IEnumerable<Participant> GetListOfParticipants();
    }

}
