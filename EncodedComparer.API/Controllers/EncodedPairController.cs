using EncodedComparer.API.Controllers;
using EncodedComparer.Domain.Commands;
using EncodedComparer.Domain.Handlers;
using EncodedComparer.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EncodedComparer.Controllers
{
    public class EncodedPairController : BaseController
    {
        private EncodedPairHandler _handler;

        public EncodedPairController(EncodedPairHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        [Route("v1/diff/{id}/left")]
        public async Task<ActionResult> Get(int id, [FromBody]SetLeftCommand command)
        {
            command.Id = id;
            var commandResult = await _handler.Handle(command);
            return ResponseBuilder(commandResult.Data, commandResult.Success, commandResult.Message, _handler.Notifications);
        }

        [HttpPost]
        [Route("v1/diff/{id}/right")]
        public async Task<ActionResult> Get(int id, [FromBody]SetRightCommand command)
        {
            command.Id = id;
            var commandResult = await _handler.Handle(command);
            return ResponseBuilder(commandResult.Data, commandResult.Success, commandResult.Message, _handler.Notifications);

        }

        [HttpGet]
        [Route("v1/diff/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var command = new FindDifferencesCommand() { Id = id };
            var commandResult = await _handler.Handle(command);
            return ResponseBuilder(commandResult.Data, commandResult.Success, commandResult.Message, _handler.Notifications);

        }
    }
}
