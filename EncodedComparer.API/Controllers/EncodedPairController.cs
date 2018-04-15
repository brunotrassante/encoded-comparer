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

        /// <summary>
        /// Register a new Left base 64 encoded JSON for future comparison.
        /// </summary>
        /// <param name="id">Integer number between 1 and 9999 used to associate with a Right pair.</param>
        /// <remarks>Once Created, it is impossible to update or delete.
        /// Sample request:
        ///
        ///   POST /v1/diff/1/left
        ///   {
        ///     "base64EncodedData": "ew0KIm5hbWUiOiJKb2huIiwNCiJhZ2UiOjMwLA0KImNhcnMiOlsgIkZvcmQiLCAiQk1XIiwgIkZpYXQiIF0NCn0"
        ///   }
        /// </remarks>
        /// <response code="200">Data added on the provided Id Left. Can never be changed.</response>
        /// <response code="400">May happend if the Id is already in use or the base64 input not valid.</response>
        /// <response code="500">Totaly unexpected error.</response>
        [HttpPost]
        [Produces("application/json")]
        [Route("v1/diff/{id}/left")]
        public async Task<ActionResult> Get(int id, [FromBody]SetLeftCommand command)
        {
            command.Id = id;
            var commandResult = await _handler.Handle(command);
            return ResponseBuilder(commandResult.Data, commandResult.Success, commandResult.Message, _handler.Notifications);
        }

        /// <summary>
        /// Register a new Right base 64 encoded JSON for future comparison.
        /// </summary>
        /// <param name="id">Integer number between 1 and 9999 used to associate with a Left pair.</param>
        /// <remarks>Once Created, it is impossible to update or delete.
        /// Sample request:
        ///
        ///   POST /v1/diff/1/right
        ///   {
        ///     "base64EncodedData": "ew0KIm5hbWUiOiJKb2huIiwNCiJhZ2UiOjMwLA0KImNhcnMiOlsgIkZvcmQiLCAiQk1XIiwgIkZpYXQiIF0NCn0"
        ///   }
        /// </remarks>
        /// <response code="200">Data added on the provided Id Right. Can never be changed.</response>
        /// <response code="400">May happend if the Id is already in use or the base64 input not valid.</response>
        /// <response code="500">Totaly unexpected error.</response>
        [HttpPost]
        [Route("v1/diff/{id}/right")]
        public async Task<ActionResult> Get(int id, [FromBody]SetRightCommand command)
        {
            command.Id = id;
            var commandResult = await _handler.Handle(command);
            return ResponseBuilder(commandResult.Data, commandResult.Success, commandResult.Message, _handler.Notifications);
        }

        /// <summary>
        /// Compare the Left and Right of an Id. If they have same size but are not equal, there will be a list of diff offsets and lenghts.
        /// </summary>
        /// <param name="id">Integer that has a Left and Right base64 data associated with it.</param>
        /// <remarks>The return is standardized. It will always provide: success, message, data and notifications.
        /// Sample result:
        /// 
        /// {
        ///  GET /v1/diff/1
        ///  "success": true,
        ///  "message": "Left and Right are same size but have differences. See the differences list.",
        ///  "data": [
        ///    {
        ///      "startingIndex": 15,
        ///      "length": 5
        ///    },
        ///    {
        ///      "startingIndex": 74,
        ///      "length": 1
        ///    }
        ///  ],
        ///  "notifications": []
        ///}
        /// </remarks>
        /// <response code="200">Data added on the provided Id Right. Can never be changed.</response>
        /// <response code="400">If the Id does not have data on Left and Right</response>
        /// <response code="500">Totaly unexpected error.</response>
        [HttpGet]
        [Produces("application/json")]
        [Route("v1/diff/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var command = new FindDifferencesCommand() { Id = id };
            var commandResult = await _handler.Handle(command);
            return ResponseBuilder(commandResult.Data, commandResult.Success, commandResult.Message, _handler.Notifications);

        }
    }
}
