using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Db;
using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.TelegramBot.Configuration;

namespace Simply.Sales.TelegramBot.Controllers {
	[Route("api/db")]
	[ApiController]
	public class DbController : Controller {
		private readonly IMediator _mediator;

		public DbController(IMediator mediator) {
			Contract.Requires(mediator != null);

			_mediator = mediator;
		}

		[HttpPost("init")]
		public async Task<IActionResult> InitializeDb() {
			try {
				await _mediator.Send(new InitDb());

				return Ok();
			}
			catch {
				return BadRequest();
			}
		}

		[HttpPost("chatIds")]
		public async Task<IActionResult> GetChatIds([FromBody] string authToken) {
			if(!AuthHelper.GetMd5Token().Equals(authToken)) {
				return BadRequest();
			}

			var chatIds = await _mediator.Send(new GetClientChatIds());
			var json = JsonSerializer.Serialize(chatIds);

			return new ContentResult { Content = json };
		}
	}
}
