using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

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
				//await _mediator.Send(new InitDb());

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


			return Ok();
		}
	}
}
