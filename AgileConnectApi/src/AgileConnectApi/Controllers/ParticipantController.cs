using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AgileConnectApi.Models;

namespace AgileConnectApi.Controllers
{
    [Route("api/[controller]")]
    public class ParticipantController : Controller
    {
        private readonly IParticipantRepository _repository;
        
        public ParticipantController(IParticipantRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Participant> GetListOfParticipants()
        {
            return _repository.GetListOfParticipants();
        }

    }
}